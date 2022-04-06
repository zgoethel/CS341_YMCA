var InteropImpl =
{
    getInputValue: function (query)
    {
        return $(query).val();
    },

    returnSelectsToZero: function ()
    {
        $("select.return-to-n1").val('-1');
    }
};