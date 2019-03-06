using System;

namespace Lusa.AddinEngine.Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool EqualWithoutCase(this string str,string equStr)
        {
            return string.Equals(str, equStr, StringComparison.OrdinalIgnoreCase);
        }

        #region Substring

        public static string Left(this string str, int startindex = 0, int length = int.MaxValue)
        {
            if (str == null && str.Length < startindex)
            {
                return "";
            }

            var maxlength = str.Length - startindex;
            if (length > maxlength)
            {
                length = maxlength;
            }

            return str.Substring(startindex, length);
        }

        #endregion

        #region Convert

        #region to int

        public static int ToInt(this string str)
        {
            return str.ToInt(-1);
        }

        public static int ToInt(this string str, int errorreturn = -1)
        {
            var result = errorreturn;
            int.TryParse(str, out result);
            return result;
        }

        public static bool IsInt(this string str)
        {
            int result;
            return int.TryParse(str, out result);
        }

        #endregion

        #region To Int16

        public static Int16 ToInt16(this string str)
        {
            return str.ToInt16(-1);
        }

        public static Int16 ToInt16(this string str, Int16 errorreturn = -1)
        {
            var result = errorreturn;
            Int16.TryParse(str, out result);
            return result;
        }

        public static bool IsInt16(this string str)
        {
            Int16 result;
            return Int16.TryParse(str, out result);
        }

        #endregion

        #region To byte

        public static byte ToByte(this string str)
        {
            return str.ToByte(byte.MinValue);
        }

        public static byte ToByte(this string str, byte errorreturn = byte.MinValue)
        {
            var result = errorreturn;
            byte.TryParse(str, out result);
            return result;
        }

        public static bool IsToByte(this string str)
        {
            byte result;
            return byte.TryParse(str, out result);
        }

        #endregion

        #region To double

        public static double ToDouble(this string str)
        {
            return str.ToDouble(-1);
        }

        public static double ToDouble(this string str, double errorreturn = -1)
        {
            var result = errorreturn;
            double.TryParse(str, out result);
            return result;
        }

        public static bool IsDouble(this string str)
        {
            double result;
            return double.TryParse(str, out result);
        }

        #endregion

        #region To long

        public static long ToLong(this string str)
        {
            return str.ToLong(-1);
        }

        public static long ToLong(this string str, long errorreturn = -1)
        {
            var result = errorreturn;
            long.TryParse(str, out result);
            return result;
        }

        public static bool IsLong(this string str)
        {
            long result;
            return long.TryParse(str, out result);
        }

        #endregion

        #region to Decimal

        public static decimal ToDecimal(this string str)
        {
            return str.ToDecimal(-1);
        }

        public static decimal ToDecimal(this string str, decimal errorreturn = -1)
        {
            var result = errorreturn;
            decimal.TryParse(str, out result);
            return result;
        }

        public static bool IsDecimal(this string str)
        {
            decimal result;
            return decimal.TryParse(str, out result);
        }

        #endregion

        #region to DateTime

        public static DateTime ToDateTime(this string str)
        {
            return str.ToDateTime(DateTime.Now);
        }

        public static DateTime ToDateTime(this string t, DateTime errorreturn)
        {
            var result = errorreturn;
            DateTime.TryParse(t, out result);
            return result;
        }

        public static string ToDateTime(this string str, string format, string errorturn = "")
        {
            DateTime result;
            if (!DateTime.TryParse(str, out result))
            {
                return errorturn;
            }
            return result.ToString(format);
        }

        public static string ToDateTime(this string str, string format)
        {
            return str.ToDateTime(format, string.Empty);
        }

        public static string ToShortDateTime(this string str)
        {
            return str.ToDateTime("yyyy-MM-dd", string.Empty);
        }

        public static bool IsDateTime(this string str)
        {
            DateTime result;
            return DateTime.TryParse(str, out result);
        }

        #endregion

        #endregion
    }
}