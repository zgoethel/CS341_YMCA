namespace CS341_YMCA.Pages;

using CS341_YMCA.Components;
using CS341_YMCA.Helpers;
using CS341_YMCA.Shared;
using CS341_YMCA.Services;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Control logic for the class viewing page and enrollment process. Includes
/// payment subsystem, detailed information views, and user-specific displays.
/// </summary>
public partial class EnrollClass : ComponentBase
{
    [Inject]
    public ClassRepository? Classes { get; set; }
    [Inject]
    public DatabaseService? Sql { get; set; }
    [Inject]
    public ClassValidationService? ClassValidation { get; set; }
    [Inject]
    public FileStorageService? FileStorage { get; set; }
    [Inject]
    public PrereqValidationService? Prereqs { get; set; }

    private SiteUserDBO _LoggedIn = new();
    /// <summary>
    /// DBO of user that is currently logged in.
    /// </summary>
    [CascadingParameter]
    protected SiteUserDBO LoggedIn
    {
        get => _LoggedIn;
        set
        {
            _LoggedIn = value;
            ReloadStateData();
        }
    }

    /// <summary>
    /// ID of class whose details are displayed.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = "";

    protected ClassDBO activeClass = new();
    protected ClassCalculateDetailsResult calculations = new();
    private PaymentModal? paymentScreen;
    private BsModal? dropModal;
    private string enrollmentError = "";
    private string photoUri = "";

    /// <summary>
    /// Loads page fields and calculations from the database.
    /// </summary>
    private void ReloadStateData()
    {
        activeClass = Classes!.Class_GetById(int.Parse(Id!)).Get()!;
        calculations = Classes!.Class_CalculateDetails(int.Parse(Id!), LoggedIn.Id).Get()!;

        try
        {
            // Load photo if one is set
            if (activeClass.ClassThumbId is not null)
            {
                using var stream = FileStorage!.RetrieveFile(activeClass.ClassPhotoId ?? 0).Get()!;
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // Use byte array to build base-64 image URI
                photoUri = $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}";
            }
            else
                photoUri = "images/not_found.svg";
        } catch (Exception)
        {
            photoUri = "images/error_thumb.svg";
        }

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Validates the user's schedule against the class'.
    /// </summary>
    /// <returns>Any conflicts as an error message.</returns>
    private string DetectConflict()
    {
        try
        {
            // Load both sets of sessions
            var userSchedule = Classes!.ClassSchedule_GetByUserId(LoggedIn.Id).Get()!;
            var classSchedule = Classes!.ClassSchedule_List(activeClass.Id).Get()!;
            // Iterate through class sessions
            foreach (var session in classSchedule)
            {
                // Validate each against user schedule
                ClassValidation!.Validate(session, userSchedule);
            }
            return "";
        } catch (Exception ex)
        {
            // Return exception message as validation warning
            return ex.Message;
        }
    }

    /// <summary>
    /// Called when enrollment button clicked to show payment modal.
    /// </summary>
    private void EnrollClick()
    {
        // Check what the user owes
        var thisUserCost = activeClass.NonMemberPrice;
        if (LoggedIn.IsMember)
            thisUserCost = activeClass.MemberPrice;
        // Show modal if costs money, or just enroll
        if (thisUserCost > 0.0m)
            paymentScreen!.StartProcess(thisUserCost);
        else
            EnrollWithPayment(null);
    }

    /// <summary>
    /// Completes the enrollment, associated with a possible payment.
    /// </summary>
    /// <param name="paymentId">ID of payment, or null for none.</param>
    private void EnrollWithPayment(int? paymentId)
    {
        // Perform enrollment and update state
        var result = Classes!.Class_EnrollUser(activeClass.Id, LoggedIn!.Id, paymentId);

        if (!result.Success)
            enrollmentError = result.Error ?? "An unexpected error occurred.";
        ReloadStateData();
    }

    /// <summary>
    /// Called when the drop dialog is accepted to perform drop.
    /// </summary>
    /// <returns></returns>
    private async Task<bool> DropDialogSubmit() => await Task.Run(() =>
    {
        // Drop the user from the class
        var result = Classes!.Class_DropUser(activeClass.Id, LoggedIn!.Id);
        ReloadStateData();

        return result.Success;
    });
}