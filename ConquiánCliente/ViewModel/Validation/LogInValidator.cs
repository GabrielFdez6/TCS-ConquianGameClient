using ConquiánCliente.Properties.Langs;

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
