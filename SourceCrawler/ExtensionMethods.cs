using System;

namespace SourceCrawler
{
    public static class ExtensionMethods
    {
        public static string SafeToString(this object ValueIn)
        {
            try
            {
                if (ValueIn == null || ValueIn == System.DBNull.Value || ValueIn.ToString() == "")
                {
                    return String.Empty;
                }
                else
                {
                    return ValueIn.ToString();
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string SafeStringToSQL(this string ValueIn)
        {
            try
            {
                if (ValueIn == null)
                {
                    return "null";
                }
                else
                {
                    return String.Format("'{0}'", ValueIn.Replace("'", "''"));
                }
            }
            catch
            {
                return "''";
            }
        }

        public static string FixForSQL(this string ValueIn)
        {
            return ValueIn.Replace("'", "''");
        }

        public static int SafeToInt32(this object ValueIn)
        {
            try
            {
                if (ValueIn == null || ValueIn == System.DBNull.Value || ValueIn.ToString() == "")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(ValueIn);
                }
            }
            catch
            {
                return 0;
            }
        }

        public static String GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff");
        }
    }
}
