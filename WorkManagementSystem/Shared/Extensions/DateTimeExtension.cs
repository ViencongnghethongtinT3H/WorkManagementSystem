using System.Globalization;

namespace WorkManagementSystem.Shared.Extensions
{
    public static class DateTimeExtension
    {
        private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static string ToddMMyyyy(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToString("dd/MM/yyyy") : "";
        }
        public static string ToddMMyyyy(this DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        public static string ToFormatString(this DateTime? dt, string format = "dd/MM/yyyy HH:mm")
        {
            return dt.HasValue ? dt.Value.ToString(format) : "";
        }

        public static string ToFormatString(this DateTime dt, string format = "dd/MM/yyyy HH:mm")
        {
            return dt.ToString(format);
        }

        public static string ToFormatStringWithGMT7(this DateTime dt, string format = "dd/MM/yyyy HH:mm")
        {
            dt = dt.AddHours(7);
            return dt.ToString(format);
        }

        public static DateTime ParseDateTimeNotNull(this string s, bool isToDate = false, string format = "dd/MM/yyyy HH:mm", CultureInfo provider = null,
     DateTimeStyles dateTimeStyles = DateTimeStyles.None)
        {
            DateTime result = default(DateTime);
            provider ??= CultureInfo.InvariantCulture;
            if (DateTime.TryParseExact(s, format, provider: provider, style: dateTimeStyles, result: out DateTime dateTime))
            {
                if (!isToDate)
                {
                    result = dateTime;
                }
                else
                {
                    result = dateTime.AddDays(1).AddTicks(-1);
                }
            }
            return result;
        }
    }
}
