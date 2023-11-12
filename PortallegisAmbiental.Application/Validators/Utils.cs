using System.Text.RegularExpressions;

namespace PortalLegisAmbiental.Application.Validators
{
    internal static class Utils
    {
        internal static bool IsEmail(string? email)
        {
            if (string.IsNullOrEmpty(email)) return true;
            var emailRx = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (!emailRx.IsMatch(email))
                return false;

            return true;
        }

        internal static bool VerifyEnum<T>(string? enumValue) where T : Enum
        {
            if (string.IsNullOrEmpty(enumValue)) return true;
            return Enum.TryParse(typeof(T), enumValue, true, out _);
        }
    }
}
