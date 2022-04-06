using System.Security.Cryptography;
using System.Text;

namespace CS341_YMCA.Helpers;

/// <summary>
/// Misc. convenience functions and helper extensions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Calculates the SHA-512 hash in base-64 encoding.
    /// </summary>
    /// <param name="value">Value to hash.</param>
    /// <returns>Base-64 encoding of the hash.</returns>
    public static string CalculateSha512(this string value)
    {
        var hashTool = SHA512.Create();
        Byte[] phraseAsBytes = Encoding.UTF8.GetBytes(string.Concat(value));
        Byte[] encryptedBytes = hashTool.ComputeHash(phraseAsBytes);
        hashTool.Clear();

        return Convert.ToBase64String(encryptedBytes);
    }
}
