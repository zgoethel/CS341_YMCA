/**
 * Javascript file for the timepicker component.
 * @author Zach Goethel
 */
var BsTimePicker =
{
    /**
     * Sets up the Bootstrap `datetimepicker` library to work with Blazor
     * components via JS interop.
     * 
     * @param reference Blazor component reference for callback.
     * @param id Unique identifier of the timepicker DOM element.
     */
    bindTimePicker: function (reference, id)
    {
        // Find and bind the element
        $(`#${id}`).datetimepicker(
        {
            // Specify the time format as 12-hour with AM/PM
            format: 'hh:mm a'
        }).click(function ()
        {
            // Replace up/down arrow icons where appropriate when clicked
            $("*[data-action='incrementHours']").addClass("oi");
            $("*[data-action='incrementHours']").addClass("oi-chevron-top");
            $("*[data-action='decrementHours']").addClass("oi");
            $("*[data-action='decrementHours']").addClass("oi-chevron-bottom");
            $("*[data-action='incrementMinutes']").addClass("oi");
            $("*[data-action='incrementMinutes']").addClass("oi-chevron-top");
            $("*[data-action='decrementMinutes']").addClass("oi");
            $("*[data-action='decrementMinutes']").addClass("oi-chevron-bottom");
        }).on("dp.change", function (event)
        {
            // Invoke callback when timepicker value changes
            reference.invokeMethodAsync("OnValueChanged", event.date);
        });
    }
};