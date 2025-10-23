using ConquiánCliente.Properties.Langs;
using System.Text.RegularExpressions;
using ConquiánCliente.ViewModel.Validation;

namespace ConquiánCliente.ViewModel.Validation
{
    public static class PasswordRecoveryValidator
    {
        public static string ValidateEmail(string email)
        {
            return SignUpValidator.ValidateEmail(email);
        }

        public static string ValidateToken(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 6 || !int.TryParse(code, out _))
            {
                return Lang.ErrorVerificationCodeIncorrect;
            }
            return string.Empty;
        }

        public static string ValidatePasswords(string password, string confirmPassword)
        {
            string passwordError = SignUpValidator.ValidatePassword(password);
            if (!string.IsNullOrEmpty(passwordError))
            {
                return passwordError;
            }

            string mismatchError = SignUpValidator.ValidateConfirmPassword(password, confirmPassword);
            if (!string.IsNullOrEmpty(mismatchError))
            {
                return mismatchError;
            }
            return string.Empty;
        }
    }
}