using ConquiánCliente.Properties.Langs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquiánCliente.ViewModel.Validation
{
    public static class LogInValidator
    {
        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Lang.ErrorEmailEmpty;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    return Lang.ErrorEmailInvalidFormat;
                }
            }
            catch
            {
                return Lang.ErrorEmailInvalidFormat;
            }
            return string.Empty;
        }

        public static string ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Lang.ErrorPasswordEmpty;
            }
            return string.Empty;
        }
    }
}
