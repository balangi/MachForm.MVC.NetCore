using System.Globalization;

namespace MachForm.NetCore.Helpers;

public static class DateHelper
{
    public static string MfRelativeDate(string inputDate)
    {
        if (string.IsNullOrEmpty(inputDate))
            return "N/A";

        DateTime date;
        if (!DateTime.TryParseExact(inputDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            return "N/A";

        TimeSpan diff = DateTime.Now - date;

        if (diff.TotalDays > 30)
            return date.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);

        if (diff.TotalDays > 1)
            return $"{(int)diff.TotalDays} days ago";

        if (diff.TotalHours > 1)
            return $"{(int)diff.TotalHours} hours ago";

        if (diff.TotalMinutes > 1)
            return $"{(int)diff.TotalMinutes} minutes ago";

        return $"{(int)diff.TotalSeconds} seconds ago";
    }

    public static string MfShortRelativeDate(string inputDate)
    {
        if (string.IsNullOrEmpty(inputDate))
            return string.Empty;

        DateTime date;
        if (!DateTime.TryParseExact(inputDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            return string.Empty;

        TimeSpan diff = DateTime.Now - date;

        if (diff.TotalDays > 30)
        {
            if (date.Year < DateTime.Now.Year)
                return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            else
                return date.ToString("MMM d", CultureInfo.InvariantCulture);
        }

        if (diff.TotalDays > 1)
            return $"{(int)diff.TotalDays} days ago";

        if (diff.TotalHours > 1)
            return $"{(int)diff.TotalHours} hours ago";

        if (diff.TotalMinutes > 1)
            return $"{(int)diff.TotalMinutes} minutes ago";

        return $"{(int)diff.TotalSeconds} seconds ago";
    }

    public static bool MfIsLeapYear(int year)
    {
        return DateTime.IsLeapYear(year);
    }
}