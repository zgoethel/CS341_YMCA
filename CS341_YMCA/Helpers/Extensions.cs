using System.Security.Cryptography;
using System.Text;

namespace CS341_YMCA.Helpers;

public static class Extensions
{
    public static string CalculateSha512(this string Value)
    {
        var HashTool = SHA512.Create();
        Byte[] PhraseAsByte = Encoding.UTF8.GetBytes(string.Concat(Value));
        Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
        HashTool.Clear();

        return Convert.ToBase64String(EncryptedBytes);
    }
}
