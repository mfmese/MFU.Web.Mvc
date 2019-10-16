using System;

namespace Citibank.MFU.Web.Business
{
    internal static class NullHelper
    {
        public static Decimal NullCheckDecimal(object input)
        {
            try
            {
                return (input != null && !(input is DBNull)) ? Convert.ToDecimal(input) : 0;
            }
            catch (Exception)
            {
                return 0;
            }
            
        }
        public static DateTime NullCheckDate(object input)
        {
            try
            {
                return (input != null && !(input is DBNull)) ? Convert.ToDateTime(input) : DateTime.MinValue;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
     
        }
    }
}
