namespace CS341_YMCA.Components;

using CS341_YMCA.Helpers;
using CS341_YMCA.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// Control for the photo picker UI component and photo uploads.
/// </summary>
public partial class PhotoPicker : ComponentBase
{
    [CascadingParameter]
    public SiteUserDBO LoggedIn { get; set; } = new();

    [Inject]
    public FileStorageService? FileStorage { get; set; }

    /// <summary>
    /// Name of the field in the UI field label.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Whether or not any image has been selected, is stored in memory, and can
    /// be saved to file.
    /// </summary>
    public bool HasValue => data is not null;

    private MemoryStream? data;
    private string? fileName;
    private string? previewUri;

    /// <summary>
    /// Invoked when a new file is selected to update the data and UI preview.
    /// </summary>
    private async Task FileSelected(InputFileChangeEventArgs e)
    {
        // Clear out old cached data if exists
        if (data is not null) data.Dispose();
        data = new MemoryStream();
        // Downscale and reformat the image to conform
        var image = await e.File.RequestImageFileAsync("image/jpeg", 1024, 1024);
        fileName = image.Name;

        // Copy the file contents to memory
        using var stream = image.OpenReadStream(512000000);
        await stream.CopyToAsync(data);
        data.Seek(0, SeekOrigin.Begin);

        // Re-render preview with new photo data
        PopulatePreview(data);
        // Return buffer to start after reading contents
        data.Seek(0, SeekOrigin.Begin);
    }

    /// <summary>
    /// Stores the file in the application's file storage and records it.
    /// </summary>
    /// <returns>Created storage record ID in the database.</returns>
    /// <exception cref="Exception">If the file upload fails.</exception>
    public int SaveImage()
    {
        if (data is null || fileName is null)
            return 0;

        // Hook into file storage subsystem
        var result = FileStorage!.StoreFile(data, fileName, "image/jpeg", LoggedIn.Id);
        if (result.Success)
            return result.Value;
        else
            throw new Exception(result.Error);
    }

    /// <summary>
    /// Replaces the internal preview image with the provided URI data.
    /// </summary>
    /// <param name="uri">Encoded image data or image path.</param>
    public void PopulatePreview(string uri)
    {
        previewUri = uri;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Replaces the internal preview image with the provided stream data.
    /// </summary>
    /// <param name="photoData">Stream containing photo's JPEG data.</param>
    public void PopulatePreview(Stream photoData)
    {
        // Read the file contents from memory into a byte array
        var bytes = new byte[photoData.Length];
        photoData.Read(bytes, 0, bytes.Length);
        // Use byte array to build base-64 image URI
        previewUri = $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}";
        InvokeAsync(StateHasChanged);
    }
}