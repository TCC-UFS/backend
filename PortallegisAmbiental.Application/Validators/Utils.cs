using System.Text.RegularExpressions;

namespace PortalLegisAmbiental.Application.Validators
{
    internal static class Utils
    {
        internal static bool IsEmail(string email)
        {
            var emailRx = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (!emailRx.IsMatch(email))
                return false;

            return true;
        }
    }
}
