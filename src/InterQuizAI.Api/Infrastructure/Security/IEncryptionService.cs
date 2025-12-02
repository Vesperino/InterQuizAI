namespace InterQuizAI.Api.Infrastructure.Security;

public interface IEncryptionService
{
    EncryptedData Encrypt(string plainText, string masterKey);
    string Decrypt(string encryptedText, string salt, string iv, string masterKey);
    string HashMasterKey(string masterKey, string salt);
    string GenerateSalt();
    bool VerifyMasterKey(string masterKey, string hash, string salt);
}

public record EncryptedData(string CipherText, string Salt, string IV);
