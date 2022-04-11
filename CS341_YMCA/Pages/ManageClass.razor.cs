namespace CS341_YMCA.Pages;

using CS341_YMCA.Components;
using CS341_YMCA.Helpers;
using CS341_YMCA.Services;
using CS341_YMCA.Shared;
using Microsoft.AspNetCore.Components;

/// <summary>
/// State and control logic for the class editing page, involving sub-components
/// such as the scheduler and prerequisite selectors..
/// </summary>
public partial class ManageClass : ComponentBase
{
    [Inject]
    public ClassRepository? Classes { get; set; }
    [Inject]
    public DatabaseService? Sql { get; set; }
    [Inject]
    public NavigationManager? Nav { get; set; }
    [Inject]
    public SiteUserRepository? SiteUsers { get; set; }
    [Inject]
    public FileStorageService? FileStorage { get; set; }

    /// <summary>
    /// DBO of the currently logged in user.
    /// </summary>
    [CascadingParameter]
    protected SiteUserDBO LoggedIn { get; set; } = new();

    /// <summary>
    /// ID of the class being edited.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    private ClassDBO activeClass = new();
    public string? validationMessage;
    private CsvSelector? fulfillSelector;
    private CsvSelector? requireSelector;
    private ClassScheduler? scheduler;
    private List<ClassEnrollmentDBO> enrolled = new();
    private BsModal? deleteModal;
    private PhotoPicker? thumbPicker;
    private PhotoPicker? photoPicker;
    private bool photosHaveLoaded = false;

    private string NonMemberStyle => activeClass.AllowNonMembers
        ? "display: block;"
        : "display: none;";

    protected override void OnInitialized()
    {
        if (Id != "Create")
        {
            // Load class into editor
            activeClass = Classes!.Class_GetById(int.Parse(Id!)).Get()!;
            // Load list of user enrollments
            enrolled = Classes.ClassEnrollment_GetByClassId(int.Parse(Id!)).Get()!;
            StateHasChanged();
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (!photosHaveLoaded && thumbPicker is not null
            && photoPicker is not null)
        {
            photosHaveLoaded = true;

            try
            {
                // Populate thumbnail picker with existing image
                if (activeClass.ClassThumbId is not null)
                {
                    using var thumbStream = FileStorage!.RetrieveFile(activeClass.ClassThumbId ?? 0).Get()!;
                    thumbPicker?.PopulatePreview(thumbStream);
                } else
                    thumbPicker!.PopulatePreview("images/not_found.svg");
            } catch (Exception)
            {
                thumbPicker!.PopulatePreview("images/error_thumb.svg");
            }

            try
            {
                // Populate photo picker with existing image
                if (activeClass.ClassPhotoId is not null)
                {
                    using var photoStream = FileStorage!.RetrieveFile(activeClass.ClassPhotoId ?? 0).Get()!;
                    photoPicker!.PopulatePreview(photoStream);
                } else
                    photoPicker!.PopulatePreview("images/not_found.svg");
            } catch (Exception)
            {
                photoPicker!.PopulatePreview("images/error_thumb.svg");
            }
        }
    }

    /// <summary>
    /// Called after delete dialog accepted to perform deletion.
    /// </summary>
    /// <returns></returns>
    private async Task<bool> DeleteDialogSubmit() => await Task.Run(() =>
    {
        // Delete class and related details
        var result = Classes!.Class_DeleteById(activeClass.Id);
        Nav!.NavigateTo("ManageClasses");

        return false;
    });

    /// <summary>
    /// Saves the class details from the editing form and schedule.
    /// </summary>
    /// <param name="redirect">Whether to send back to class list.</param>
    public void SaveForm(bool redirect = true)
    {
        // Validation for required text fields
        if (string.IsNullOrEmpty(activeClass.ClassName)
            || string.IsNullOrEmpty(activeClass.ShortDescription))
        {
            validationMessage = "Please provide all required fields.";
            StateHasChanged();
            return;
        }
        // Validation for pricing values
        if (activeClass.MemberPrice < 0
            || activeClass.NonMemberPrice < 0)
        {
            validationMessage = "Please use positive dollar amounts.";
            StateHasChanged();
            return;
        }
        // Validation for seat limit value
        if ((activeClass.MaxSeats ?? 0) < 0
            || (activeClass.MaxSeats ?? 0) != (int)(activeClass.MaxSeats ?? 0))
        {
            validationMessage = "Please use whole, positive number of max seats.";
            StateHasChanged();
            return;
        }

        var sessions = scheduler!.schedule;//Classes.ClassSchedule_List(ActiveClass.Id).Get()!;
        if (sessions.Count > 0
            && activeClass.MemberEnrollmentStart != null
            && (activeClass.NonMemberEnrollmentStart != null
            || !activeClass.AllowNonMembers))
        {
            // Validate the first session is after enrollment period
            var enrollmentEnd = activeClass.MemberEnrollmentStart!.Value.AddDays(activeClass.MemberEnrollmentDays ?? 1);
            if (activeClass.AllowNonMembers)
            {
                var _enrollmentEnd = activeClass.NonMemberEnrollmentStart!.Value.AddDays(activeClass.NonMemberEnrollmentDays ?? 1);
                if (_enrollmentEnd > enrollmentEnd)
                    enrollmentEnd = _enrollmentEnd;
            }

            if (enrollmentEnd > sessions[0].FirstDate)
            {
                validationMessage = "Classes cannot start before enrollment closes. Please change the enrollment window(s) or class schedule date(s).";
                StateHasChanged();
                return;
            }
        }

        // Validation for requiring enrollment dates
        if (activeClass.AllowEnrollment)
        {
            // Ensure that enrollment is set up if opened
            if (activeClass.MemberEnrollmentStart == null
                || (activeClass.MemberEnrollmentDays ?? 0) == 0)
            {
                validationMessage = "You cannot \"Allow Enrollment\" without first setting an enrollment window.";
                StateHasChanged();
                return;
            } else if (activeClass.MemberEnrollmentDays != null
                && (activeClass.MemberEnrollmentDays != (int)activeClass.MemberEnrollmentDays
                || activeClass.MemberEnrollmentDays < 0))
            {
                validationMessage = "Specify enrollment window length as a number of whole days.";
                StateHasChanged();
                return;
            }

            // Ensure enrollment set up for non-members if applicable
            if (activeClass.AllowNonMembers
                && (activeClass.MemberEnrollmentStart == null
                || (activeClass.NonMemberEnrollmentDays ?? 0) == 0))
            {
                validationMessage = "Please either disable non-members, or specify their enrollment window.";
                StateHasChanged();
                return;
            } else if (activeClass.AllowNonMembers
                && activeClass.NonMemberEnrollmentDays != null
                && (activeClass.NonMemberEnrollmentDays != (int)activeClass.NonMemberEnrollmentDays
                || activeClass.NonMemberEnrollmentDays < 0))
            {
                validationMessage = "Specify enrollment window length as a number of whole days.";
                StateHasChanged();
                return;
            }
        }

        if (fulfillSelector!.selected.Intersect(requireSelector!.selected).Any())
        {
            validationMessage = "Each requirement code may only be used once, in either requirements of fullfillments.";
            StateHasChanged();
            return;
        }
        if (activeClass.MaxSeats != 0 && activeClass.MaxSeats < enrolled.Count)
        {
            validationMessage = "The maximum number of seats must not be lower than the number of currently enrolled patrons.";
            StateHasChanged();
            return;
        }

        // Store the validated values to the database
        var Result = Classes!.Class_Set(
            Id: activeClass.Id,
            ClassName: activeClass.ClassName,
            AllowEnrollment: activeClass.AllowEnrollment,
            Enabled: activeClass.Enabled,
            ShortDescription: activeClass.ShortDescription,
            LongDescription: activeClass.LongDescription,
            MemberEnrollmentStart: activeClass.MemberEnrollmentStart,
            MemberEnrollmentDays: activeClass.MemberEnrollmentDays,
            NonMemberEnrollmentStart: activeClass.NonMemberEnrollmentStart,
            NonMemberEnrollmentDays: activeClass.NonMemberEnrollmentDays,
            AllowNonMembers: activeClass.AllowNonMembers,
            MemberPrice: activeClass.MemberPrice,
            NonMemberPrice: activeClass.NonMemberPrice,
            Location: activeClass.Location,
            MaxSeats: activeClass.MaxSeats,
            FulfillCsv: fulfillSelector.Csv,
            RequireCsv: requireSelector.Csv,
            ClassThumbId: thumbPicker!.HasValue ? thumbPicker!.SaveImage() : null,
            ClassPhotoId: photoPicker!.HasValue ? photoPicker!.SaveImage() : null
        );

        // Write back created (or returned) ID
        activeClass.Id = Result.Get()!;
        // Save schedule as well
        if (Result.Success) scheduler.Save();

        if (Result.Success && redirect)
        {
            // Redirect after save
            Nav!.NavigateTo("ManageClasses");
        } else if (Result.Success)
        {
            // Clear validation message after save
            validationMessage = "";
            StateHasChanged();
        } else
        {
            // Display an error after attempt
            validationMessage = Result.Error ?? "An unknown error has occurred.";
            StateHasChanged();
        }
    }
}