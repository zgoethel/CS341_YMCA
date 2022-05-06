/**
 * Logic which enables the prerequisite selector component.
 * @author Zach Goethel
 */

namespace CS341_YMCA.Shared;

using CS341_YMCA.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;

/// <summary>
/// Control logic for the CSV prereq selector component.
/// </summary>
public partial class CsvSelector : ComponentBase
{
    [Inject]
    private IJSRuntime? Js { get; set; }

    /// <summary>
    /// Layout of bound editing form data.
    /// </summary>
    private class BindModel
    {
        public string Value { get; set; } = "";
    }

    /// <summary>
    /// Object instance bound to editing form.
    /// </summary>
    private readonly BindModel formBind = new();

    /// <summary>
    /// Initially selected CSV for the items.
    /// </summary>
    [Parameter]
    public string? Csv { get; set; }

    /// <summary>
    /// Items to ignore entirely from the options (e.g., a class shouldn't
    /// require itself).
    /// </summary>
    [Parameter]
    public List<string> Exclude { get; set; } = new();

    /// <summary>
    /// Items from which to select (strings).
    /// </summary>
    [Parameter]
    public List<string> Items { get; set; } = new();

    /// <summary>
    /// Called when an item is selected with the name of the item.
    /// </summary>
    [Parameter]
    public Action? Callback { get; set; }

    public List<string> selected = new();
    private List<string> notSelected = new();
    private string validationMessage = "";
    private BsModal? addCodeModal;

    /// <summary>
    /// Called when the modal is submitted to create a new code.
    /// </summary>
    /// <returns>Whether the modal should close.</returns>
    private async Task<bool> AddCodeSubmit() => await Task.Run(() =>
    {
        // Check format of provided item
        if (!Regex.IsMatch(formBind.Value, "^[A-Za-z0-9]+$"))
        {
            validationMessage = "Please enter an alphanumeric class code.";
            InvokeAsync(() => StateHasChanged());

            return false;
        }

        // Convert to upper case for consistency
        var item = formBind.Value.ToUpper();

        // Don't do anything if the item is already selected
        if (selected.Contains(item))
            return false;
        // Add to selected list
        selected.Add(item);
        // Remove from not-selected list
        notSelected.Remove(item);

        // Reset modal state
        validationMessage = "";
        formBind.Value = "";
        // Regenerate item CSV from selected list
        Csv = String.Join(',', selected);

        InvokeAsync(() =>
        {
            StateHasChanged();
            Callback?.Invoke();
        });

        return true;
    });

    protected override void OnInitialized()
    {
        base.OnInitialized();

        // Convert selected list to hash set for "contains" checks
        var csvLookup = Csv!.Split(",").ToHashSet();
        // Create selected/not-selected lists
        selected = Csv!.Split(",").ToList();
        if (string.IsNullOrEmpty(Csv!)) selected.Clear();
        notSelected = Items.FindAll((It) => !csvLookup.Contains(It) && !Exclude.Contains(It));
    }

    /// <summary>
    /// Removes the provided item from the set of selected items.
    /// </summary>
    /// <param name="item">CSV item to remove from the selected list.</param>
    private void OnRemoveClick(string item)
    {
        selected.Remove(item);
        notSelected.Add(item);
        // Regenerate CSV from selected list
        Csv = string.Join(',', selected);

        InvokeAsync(() =>
        {
            StateHasChanged();
            Callback?.Invoke();
        });
    }

    /// <summary>
    /// Adds an item to the set of selected items or performs an action.
    /// </summary>
    /// <param name="item">CSV item to ad to the selected list.</param>
    private void OnAddSelectorChange(ChangeEventArgs e)
    {
        var item = e.Value!.ToString()!;

        // Handle special actions (-1 = ignore)
        if (item == "-1") return;
        // (-2 = open create modal)
        if (item == "-2")
            addCodeModal!.Open();
        else
        {
            // Normal behavior; add item to list
            selected.Add(item);
            notSelected.Remove(item);
            // Regenerate CSV from selected list
            Csv = string.Join(',', selected);
        }

        // Resets the selector to "Add +"
        Js!.InvokeVoidAsync("InteropImpl.returnSelectsToZero");

        InvokeAsync(() =>
        {
            StateHasChanged();
            Callback?.Invoke();
        });
    }
}