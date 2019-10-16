using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citibank.MFU.Web.Business
{
    internal static class StringHelper
    {
        public static StringBuilder GetStringBuilder(params object[] values)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                string value = ConvertToString(values[i]);
                stringBuilder.AppendLine(value);
            }
            return stringBuilder;
        }

        public static string ConvertToString(object value)
        {
            if (value == null)
                return String.Empty;

            if (value is DateTime)
                return ConvertToString((DateTime)value);

            if (value is decimal)
                return ConvertToString((decimal)value);

            return value.ToString();
        }

        public static byte[] ConvertToByteArray(StringBuilder input)
        {
            return ConvertToByteArray(input.ToString());
        }
        public static byte[] ConvertToByteArray(string input)
        {
            return Encoding.Default.GetBytes(input);
        }
    }
}
