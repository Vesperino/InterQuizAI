using System.Security.Cryptography;
using System.Text;

namespace InterQuizAI.Api.Infrastructure.Security;

public class AesEncryptionService : IEncryptionService
{
    private const int SaltSize = 32;
    private const int KeySize = 32; // 256 bits
    private const int IvSize = 16;  // 128 bits
    private const int Iterations = 100000;

    public EncryptedData Encrypt(string plainText, string masterKey)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] iv = RandomNumberGenerator.GetBytes(IvSize);

        byte[] key = DeriveKey(masterKey, salt);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return new EncryptedData(
            Convert.ToBase64String(cipherBytes),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(iv)
        );
    }

    public string Decrypt(string encryptedText, string salt, string iv, string masterKey)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] ivBytes = Convert.FromBase64String(iv);
        byte[] cipherBytes = Convert.FromBase64String(encryptedText);

        byte[] key = DeriveKey(masterKey, saltBytes);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = ivBytes;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }

    public string HashMasterKey(string masterKey, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] hash = DeriveKey(masterKey, saltBytes);
        return Convert.ToBase64String(hash);
    }

    public string GenerateSalt()
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        return Convert.ToBase64String(salt);
    }

    public bool VerifyMasterKey(string masterKey, string hash, string salt)
    {
        string computedHash = HashMasterKey(masterKey, salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(computedHash),
            Convert.FromBase64String(hash)
        );
    }

    private static byte[] DeriveKey(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256
        );
        return pbkdf2.GetBytes(KeySize);
    }
}
