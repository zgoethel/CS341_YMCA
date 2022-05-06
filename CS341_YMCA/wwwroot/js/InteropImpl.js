/**
 * Javascript file for accessing jQuery methods.
 * @author Zach Goethel
 */
var InteropImpl =
{
    /**
     * Retrieves the field value of an input DOM element.
     * 
     * @param query Query selector to find the appropriate DOM element.
     * @returns The input's internal field value.
     */
    getInputValue: function (query)
    {
        return $(query).val();
    },

    /**
     * Resets `select` elements to the value "-1`, which should be mapped to a
     * hidden dropdown option called "Add +".
     */
    returnSelectsToZero: function ()
    {
        $("select.return-to-n1").val('-1');
    }
};