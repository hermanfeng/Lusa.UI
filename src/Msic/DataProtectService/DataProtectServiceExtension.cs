using System;
using System.Text;

namespace Lusa.UI.Msic.DataProtectService
{
    public static class DataProtectServiceExtension
    {
        public static string ProtectData(this IDataProtectService service, string input, string key = "")
        {
            var bytes = UTF8Encoding.UTF8.GetBytes(input);
            var resultBytes = service.ProtectData(bytes, key);
            return Convert.ToBase64String(resultBytes);
        }

        public static string UnprotectData(this IDataProtectService service, string input, string key = "")
        {
            var bytes = Convert.FromBase64String(input);
            var resultBytes = service.UnprotectData(bytes, key);
            return UTF8Encoding.UTF8.GetString(resultBytes);
        }
    }
}
