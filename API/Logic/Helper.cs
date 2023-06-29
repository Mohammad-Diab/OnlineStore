namespace OnlineStore
{
    public static class Helper
    {
        public static string DateTimeToChar14(DateTime date)
        {
            return date.ToString("yyyyMMddHHmmss");
        }

        public static DateTime Char14ToDateTime(string date, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(date))
            {
                return defaultValue;
            }
            _ = int.TryParse(date.AsSpan(0, 4), out int year);
            _ = int.TryParse(date.AsSpan(4, 2), out int month);
            _ = int.TryParse(date.AsSpan(6, 2), out int day);
            if (date.Length > 8)
            {
                _ = int.TryParse(date.AsSpan(8, 2), out int hour);
                _ = int.TryParse(date.AsSpan(10, 2), out int minute);
                _ = int.TryParse(date.AsSpan(12, 2), out int second);
                if (year > 0 && month > 0 && day > 0 && minute > 0 && second > 0)
                {
                    return new DateTime(year, month, day, hour, minute, second);
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                if (year > 0 && month > 0 && day > 0)
                {
                    return new DateTime(year, month, day);
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        internal static string ToStandardDate(string birthday)
        {
            var date = Char14ToDateTime(birthday, DateTime.Now);
            return date.ToString("yyyy-MM-dd");
        }
    }
}
