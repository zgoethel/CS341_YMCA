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
    /// <param name="Value">Value to hash.</param>
    /// <returns>Base-64 encoding of the hash.</returns>
    public static string CalculateSha512(this string Value)
    {
        var HashTool = SHA512.Create();
        Byte[] PhraseAsByte = Encoding.UTF8.GetBytes(string.Concat(Value));
        Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
        HashTool.Clear();

        return Convert.ToBase64String(EncryptedBytes);
    }
}
