namespace CS341_YMCA.Components;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Control logic for the Bootstrap modal UI component.
/// </summary>
public partial class BsModal : ComponentBase
{
    /// <summary>
    /// Provide this field to add custom components to the modal header.
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    /// Provide this field to add header text to the modal (can be used in
    /// conjunction with the `Header` render fragment).
    /// </summary>
    [Parameter]
    public RenderFragment? Title { get; set; }

    /// <summary>
    /// Provide this field to add custom components to the modal body.
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }

    /// <summary>
    /// Provide this field to add buttons and custom components to the modal
    /// footer.
    /// </summary>
    [Parameter]
    public RenderFragment? Footer { get; set; }

    /// <summary>
    /// The text which list displayed in the modal footer's submit button.
    /// </summary>
    [Parameter]
    public string? SubmitText { get; set; } = "Submit";

    /// <summary>
    /// The text which is displayed in the modal footer's submit button.
    /// </summary>
    [Parameter]
    public string? CancelText { get; set; } = "Cancel";

    /// <summary>
    /// Action called when the modal's submit button is pressed.
    /// </summary>
    [Parameter]
    public Func<Task<bool>>? SubmitAction { get; set; }

    [Parameter]
    public string SubmitClass { get; set; } = "primary";

    /// <summary>
    /// Action called when the modal's cancel button is pressed.
    /// </summary>
    [Parameter]
    public Func<Task<bool>>? CancelAction { get; set; }

    private string errorMessage = "";
    private bool isOpen = false;

    /// <summary>
    /// Displays the modal component by rendering its static content.
    /// </summary>
    public void Open()
    {
        isOpen = true;
        InvokeAsync(() => StateHasChanged());
    }

    /// <summary>
    /// Hides the modal component by removing its static content.
    /// </summary>
    public void Close()
    {
        isOpen = false;
        InvokeAsync(() => StateHasChanged());
    }

    /// <summary>
    /// Invokes the submit action and closes the modal if requested.
    /// </summary>
    public async void Submit()
    {
        var shouldClose = SubmitAction is null
            ? true
            : (await SubmitAction.Invoke());
        if (shouldClose) Close();
    }

    /// <summary>
    /// Invokes the cancel action and closes the modal if requested.
    /// </summary>
    public async void Cancel()
    {
        var shouldClose = CancelAction is null
            ? true
            : (await CancelAction.Invoke());
        if (shouldClose) Close();
    }

    /// <summary>
    /// Displays an error or validation message in the modal body.
    /// </summary>
    /// <param name="Message">Error message which is displayed.</param>
    public void Error(string Message)
    {
        this.errorMessage = Message;
        InvokeAsync(() => StateHasChanged());
    }
}