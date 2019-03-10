using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lusa.UI.Msic.DataProtectService
{

    public class DataProtectService : IDataProtectService
    {
        static IDataProtectService instance = new DataProtectService();
        private DataProtectService()
        {

        }

        public static IDataProtectService Instance
        {
            get
            {
                return instance;
            }
        }

        private string innerKey = "LusaUIEncryption";
        public byte[] ProtectData(byte[] data, string key = "")
        {
            key = key ?? string.Empty;
            key = key + innerKey;

            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.BlockSize = aesManaged.LegalBlockSizes[0].MaxSize;
                aesManaged.KeySize = aesManaged.LegalKeySizes[0].MaxSize;
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("lsuaPassword", Encoding.UTF8.GetBytes(key));
                aesManaged.Key = rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 8);
                aesManaged.IV = rfc2898DeriveBytes.GetBytes(aesManaged.BlockSize / 8);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform transform = aesManaged.CreateEncryptor())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.Flush();
                            cryptoStream.Close();
                        }
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] UnprotectData(byte[] data, string key = "")
        {
            key = key ?? string.Empty;
            key = key + innerKey;

            if (data == null)
            {
                return null;
            }
            try
            {
                using (AesManaged aesManaged = new AesManaged())
                {
                    aesManaged.BlockSize = aesManaged.LegalBlockSizes[0].MaxSize;
                    aesManaged.KeySize = aesManaged.LegalKeySizes[0].MaxSize;
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("lsuaPassword", Encoding.UTF8.GetBytes(key));
                    aesManaged.Key = rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 8);
                    aesManaged.IV = rfc2898DeriveBytes.GetBytes(aesManaged.BlockSize / 8);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (ICryptoTransform transform = aesManaged.CreateDecryptor())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(data, 0, data.Length);
                                cryptoStream.Flush();
                                cryptoStream.Close();
                            }
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Decryption Error. There might be a malicious access." + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
