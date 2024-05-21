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
    }
}
