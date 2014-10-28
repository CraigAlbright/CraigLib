using System;
using System.Security.Cryptography;
using System.Text;

namespace CraigLib
{
    public class CryptoHelper : ICryptoHelper
    {
        private readonly RC2CryptoServiceProvider _rc2 = new RC2CryptoServiceProvider();
        private readonly DESCryptoServiceProvider _des = new DESCryptoServiceProvider();
        private readonly byte[] _key =
        {
            87,
            3,
            120,
            0,
            54,
            16,
            98,
            2
        };
        private readonly byte[] _iv =
        {
            14,
            5,
            6,
            8,
            65,
            58,
            210,
            78
        };
        private readonly UnicodeEncoding _ue = new UnicodeEncoding();
        private readonly UTF8Encoding _ae = new UTF8Encoding();
        private int _result;
        private readonly ICryptoTransform _rc2Trans;
        private readonly ICryptoTransform _rc2Enc;
        [ThreadStatic]
        private static SHA256 _sha;

        public CryptoHelper()
        {
            _des.Key = _key;
            _des.IV = _iv;
            _des.Mode = CipherMode.ECB;
            _des.Padding = PaddingMode.Zeros;
            //var cryptoServiceProvider = new RC2CryptoServiceProvider();
            var pdb = new PasswordDeriveBytes(_ae.GetString(new byte[]
            {
                14,
                5,
                6,
                8,
                65,
                58,
                78
            }), _key, "SHA-1", 1, new CspParameters(1, "Microsoft Base Cryptographic Provider v1.0"));
            _rc2.Key = pdb.CryptDeriveKey("RC2", "SHA1", 0, _iv);
            _rc2.IV = new byte[8];
            _rc2.Padding = PaddingMode.PKCS7;
            _rc2Trans = _rc2.CreateDecryptor();
            _rc2Enc = _rc2.CreateEncryptor();
        }

        public string Encrypt(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            var numArray1 = new byte[0];
            byte[] numArray2;
            try
            {
                var encryptor = _des.CreateEncryptor(_key, _iv);
                var bytes = _ue.GetBytes(Convert.ToBase64String(_ue.GetBytes(s)));
                numArray2 = new byte[bytes.Length + 16];
                _result = encryptor.TransformBlock(bytes, 0, bytes.Length, numArray2, 0);
            }
            catch
            {
                return "";
            }
            return Convert.ToBase64String(numArray2, 0, _result);
        }

        public string Decrypt(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            var numArray1 = new byte[0];
            byte[] numArray2;
            try
            {
                var decryptor = _des.CreateDecryptor(_key, _iv);
                var inputBuffer = Convert.FromBase64String(s);
                numArray2 = new byte[inputBuffer.Length + 16];
                _result = decryptor.TransformBlock(inputBuffer, 0, inputBuffer.Length, numArray2, 0);
            }
            catch
            {
                return "";
            }
            return _ue.GetString(Convert.FromBase64String(_ue.GetString(numArray2, 0, _result)));
        }

        public byte[] RC2Encrypt(string input)
        {
            return RC2Encrypt(_ae.GetBytes(input));
        }

        public byte[] RC2Encrypt(byte[] input)
        {
            return _rc2Enc.TransformFinalBlock(input, 0, input.Length);
        }

        public string RC2Decrypt(byte[] input)
        {
            var bytes = _rc2Trans.TransformFinalBlock(input, 0, input.Length);
            return _ae.GetString(bytes, 0, bytes.Length);
        }

        public string RC2EncryptB64(string input)
        {
            return Convert.ToBase64String(RC2Encrypt(input));
        }

        public string RC2Decrypt(string b64Str)
        {
            return RC2Decrypt(Convert.FromBase64String(b64Str));
        }

        public static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(ComputeHash(bytes, 0, bytes.Length));
        }

        public static byte[] ComputeHash(byte[] input, int startpos, int endpos)
        {
            if (_sha == null)
                _sha = SHA256.Create();
            return _sha.ComputeHash(input, startpos, endpos);
        }
    }
}
