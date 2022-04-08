using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace CS341_YMCA.Helpers;

/// <summary>
/// Interface for calling stored procedures in the database. Parameters' and
/// result rows' schema are defined as object class types.
/// </summary>
public class Database
{
    /// <summary>
    /// SQL-specific region of the configuration file.
    /// </summary>
    private class DatabaseConfigSection
    {
        public string InstanceName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string InitialCatalog { get; set; } = "";
    }
    /// <summary>
    /// Object bound to the database settings value section.
    /// </summary>
    private readonly DatabaseConfigSection configSection = new();

    public Database(IConfiguration Configuration)
    {
        // Bind the section of the config to the object for access
        Configuration.GetSection("SqlServer").Bind(configSection);
    }

    /// <summary>
    /// Calls the specified stored procedure in the database. The parameters are
    /// converted into a set of SQL input parameters, and the resulting rows are
    /// provided individually via invocations of a consumer function.
    /// </summary>
    /// <typeparam name="T">Return value type (parsed from results).</typeparam>
    /// <param name="procName">Name of the stored procedure.</param>
    /// <param name="parameters">Object containing invocation parameters.</param>
    /// <param name="readConsumer">Function to call and acccept result rows.</param>
    /// <exception cref="Exception">If an SQL or network error occurs.</exception>
    public void ExecuteProcedure<T>(string procName, object parameters, Action<T> readConsumer)
    {
        var Builder = new SqlConnectionStringBuilder()
        {
            DataSource = configSection.InstanceName,
            UserID = configSection.Username,
            Password = configSection.Password,
            InitialCatalog = configSection.InitialCatalog
        };

        // Open SQL connection (scoped)
        using var Sql = new SqlConnection(Builder.ConnectionString);
        Sql.Open();
        // Create stored procedure (scoped)
        using var Command = new SqlCommand(procName, Sql);
        Command.CommandType = CommandType.StoredProcedure;

        // Convert parameters into a map
        var Serialized = JsonConvert.SerializeObject(parameters);
        var AsMap = JsonConvert.DeserializeObject<Dictionary<string, object?>>(Serialized);
                
        // Non-null assertion
        AsMap = AsMap ?? throw new Exception("Could not convert parameter object to map");
        // Insert entire param map into param list
        foreach (var KeyValue in AsMap)
            Command.Parameters.AddWithValue('@' + KeyValue.Key, KeyValue.Value);
        // Open results reader (scoped)
        using var Reader = Command.ExecuteReader();
        
        while (Reader.Read())
        {
            // Create a map from the result row
            var Assemble = new Dictionary<string, object?>();
            for (int i = 0; i < Reader.FieldCount; i++)
                Assemble[Reader.GetName(i)] = Reader.GetValue(i);
            // Convert the map into an object
            Serialized = JsonConvert.SerializeObject(Assemble);
            var AsObject = JsonConvert.DeserializeObject<T>(Serialized);

            // Non-null assertion
            AsObject = AsObject ?? throw new Exception("Could not convert result row to object");
            // Invoke the read consumer callback with the row
            readConsumer.Invoke(AsObject);
        }
    }
}
