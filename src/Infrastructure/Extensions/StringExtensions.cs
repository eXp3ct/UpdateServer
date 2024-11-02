using System.Text.RegularExpressions;

namespace Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string VersionToKebabFormat(this string value)
        {
            var regex = "^\\d+\\.\\d+\\.\\d+\\.\\d+$";

            if (!Regex.IsMatch(value, regex))
                throw new ArgumentException("Input value is not like 1.1.1.1");

            var numbers = value.Split('.');

            return $"v{numbers[0]}-{numbers[1]}-{numbers[2]}-{numbers[3]}";
        }
    }
}