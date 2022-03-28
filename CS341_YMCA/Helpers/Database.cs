using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace CS341_YMCA.Helpers;

public class Database
{
    private class DatabaseConfigSection
    {
        public string InstanceName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string InitialCatalog { get; set; } = "";
    }

    private readonly DatabaseConfigSection ConfigSection = new();

    public Database(IConfiguration Configuration)
    {
        Configuration.GetSection("SqlServer").Bind(ConfigSection);
    }

    public void ExecuteProcedure<T>(string ProcName, object Parameters, Action<T> ReadConsumer)
    {
        var Builder = new SqlConnectionStringBuilder()
        {
            DataSource = ConfigSection.InstanceName,
            UserID = ConfigSection.Username,
            Password = ConfigSection.Password,
            InitialCatalog = ConfigSection.InitialCatalog
        };

        using var Sql = new SqlConnection(Builder.ConnectionString);
        Sql.Open();

        using var Command = new SqlCommand(ProcName, Sql);
        Command.CommandType = CommandType.StoredProcedure;

        var Serialized = JsonConvert.SerializeObject(Parameters);
        var AsMap = JsonConvert.DeserializeObject<Dictionary<string, object?>>(Serialized);
                
        // Non-null assertion
        AsMap = AsMap ?? throw new Exception("Could not convert parameter object to map");
        foreach (var KeyValue in AsMap)
            Command.Parameters.AddWithValue('@' + KeyValue.Key, KeyValue.Value);

        using var Reader = Command.ExecuteReader();
        
        while (Reader.Read())
        {
            var Assemble = new Dictionary<string, object?>();
            for (int i = 0; i < Reader.FieldCount; i++)
                Assemble[Reader.GetName(i)] = Reader.GetValue(i);

            Serialized = JsonConvert.SerializeObject(Assemble);
            var AsObject = JsonConvert.DeserializeObject<T>(Serialized);

            // Non-null assertion
            AsObject = AsObject ?? throw new Exception("Could not convert result row to object");
            ReadConsumer.Invoke(AsObject);
        }
    }
}
