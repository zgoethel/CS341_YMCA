var BsTimePicker =
{
    bindTimePicker: function (reference, id)
    {
        $(`#${id}`).datetimepicker(
        {
            format: 'hh:mm a'
        }).click(function ()
        {
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
            reference.invokeMethodAsync("OnValueChanged", event.date);
        });
    }
};