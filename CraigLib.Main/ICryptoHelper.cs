namespace CraigLib
{
    public interface ICryptoHelper
    {
        string Encrypt(string s);
        string Decrypt(string s);
        byte[] RC2Encrypt(string input);
        byte[] RC2Encrypt(byte[] input);
        string RC2Decrypt(byte[] input);
        string RC2EncryptB64(string input);
        string RC2Decrypt(string b64Str);
    }
}