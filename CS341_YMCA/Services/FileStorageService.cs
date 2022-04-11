namespace CS341_YMCA.Services;

using CS341_YMCA.Helpers;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Interface for saving and retrieving files from the application's storage.
/// </summary>
public class FileStorageService
{
    private readonly DatabaseService Sql;

    private readonly string Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
    private bool IsDev => Env.Equals("Development");

    /// <summary>
    /// File storage configuration in the main application config.
    /// </summary>
    private class FileStorageConfigSection
    {
        public string FolderPath { get; set; } = "FileStorage";
    }
    /// <summary>
    /// Object bound to the database settings value section.
    /// </summary>
    private readonly FileStorageConfigSection configSection = new();

    public FileStorageService(IConfiguration Configuration, DatabaseService Sql)
    {
        this.Sql = Sql;

        // Bind the section of the config to the object for access
        Configuration.GetSection("FileStorage").Bind(configSection);
    }

    /// <summary>
    /// Stores a new file and records it on the file manifest table.
    /// </summary>
    public ResultToken<int> StoreFile(
        Stream data,
        string originalName,
        string mimeType,
        int? uploadedBy = null)
    {
        var result = new ResultToken<int>();
        var storedName = Guid.NewGuid().ToString() + Path.GetExtension(originalName);

        var dir = Directory.CreateDirectory(configSection.FolderPath);
        using var file = File.Create(Path.Combine(configSection.FolderPath, storedName));
        data.CopyTo(file);

        try
        {
            Sql.ExecuteProcedure<FileStorageEnterResult>(
                "FileStorage_Enter",
                new FileStorageEnterRequest()
                {
                    StoredName = storedName,
                    OriginalName = originalName,
                    SizeBytes = (int)data.Length,
                    MimeType = mimeType,
                    UploadedBy = uploadedBy
                }, (_result) =>
                {
                    result.Value = _result.Id;
                });
        } catch (SqlException ex)
        {
            result.Success = false;
            result.Error = ex.Message;
        } catch (Exception ex)
        {
            result.Success = false;
            result.Error = IsDev ? ex.Message : "An unexpected error has occurred.";
        }

        return result;
    }

    /// <summary>
    /// Finds the specified file in application storage and returns an in-memory
    /// buffer of read data.
    /// </summary>
    /// <param name="Id">File record unique identifier.</param>
    /// <returns>Stream containing requested file data.</returns>
    public ResultToken<Stream> RetrieveFile(int Id)
    {
        var result = new ResultToken<Stream>();
        FileStorageDBO? file = null;

        try
        {
            Sql.ExecuteProcedure<FileStorageDBO>(
                "FileStorage_GetById",
                new { Id },
                (_result) =>
                {
                    file = _result;
                });
        } catch (SqlException ex)
        {
            result.Success = false;
            result.Error = ex.Message;

            return result;
        } catch (Exception ex)
        {
            result.Success = false;
            result.Error = IsDev ? ex.Message : "An unexpected error has occurred.";

            return result;
        }
        // Throw a "file not found" type of exception
        if (file is null)
            throw new Exception("Could not find the file specified.");

        // Open stored file for reading
        using var handle = File.OpenRead(Path.Combine(configSection.FolderPath, file!.StoredName));
        result.Value = new MemoryStream();
        // Copy into returned memory stream
        handle.CopyTo(result.Value);
        result.Value.Seek(0, SeekOrigin.Begin);

        return result;
    }
}
