using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConquiánCliente.Properties.Langs;

namespace ConquiánCliente.ViewModel.Validation
{
    public static class SignUpValidator
    {
        private const int MAX_NAME_LENGTH = 25;
        private const int MAX_LAST_NAME_LENGTH = 50;
        private const int MAX_NICKNAME_LENGTH = 15;
        private const int MAX_EMAIL_LENGTH = 45;
        private const int MIN_PASSWORD_LENGTH = 8;
        private const int MAX_PASSWORD_LENGTH = 15;

        public static string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Lang.ErrorNameEmpty;
            }

            if (name.Length > MAX_NAME_LENGTH)
            {
                return string.Format(Lang.ErrorNameLength, MAX_NAME_LENGTH);
            }

            if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                return Lang.ErrorValidName;
            }

            return string.Empty;
        }

        public static string ValidateLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                return Lang.ErrorLastNameEmpty;
            }
            lastName = lastName.Trim();

            if (lastName.Length > MAX_LAST_NAME_LENGTH)
            {
                return string.Format(Lang.ErrorLastNameLength, MAX_LAST_NAME_LENGTH);
            }

            if (!Regex.IsMatch(lastName, @"^[a-zA-Z\s]+$"))
            {
                return Lang.ErrorLastNameInvalidChars;
            }

            return string.Empty;
        }

        public static string ValidateNickname(string nickname)
        {
            if (string.IsNullOrEmpty(nickname))
            {
                return Lang.ErrorNicknameEmpty;
            }
            nickname = nickname.Trim();

            if (nickname.Length > MAX_NICKNAME_LENGTH)
            {
                return string.Format(Lang.ErrorNicknameLength, MAX_NICKNAME_LENGTH);
            }

            if (!Regex.IsMatch(nickname, @"^[a-zA-Z0-9]+$"))
            {
                return Lang.ErrorNicknameInvalidChars;
            }

            return string.Empty;
        }

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Lang.ErrorEmailEmpty;
            }

            if (email.Length > MAX_EMAIL_LENGTH)
            {
                return string.Format(Lang.ErrorEmailLenght, MAX_EMAIL_LENGTH);
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address == email)
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Format(Lang.ErrorEmailInvalidFormat);
            }

            return string.Format(Lang.ErrorEmailInvalidFormat);
        }

        public static string ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Lang.ErrorPasswordEmpty;
            }

            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
            {
                return string.Format(Lang.ErrorPasswordLength, MIN_PASSWORD_LENGTH, MAX_PASSWORD_LENGTH);
            }

            if (password.Contains(" "))
            {
                return Lang.ErrorPasswordNoSpaces;
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return Lang.ErrorPasswordNoUppercase;
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            {
                return Lang.ErrorPasswordNoSpecialChar;
            }

            return string.Empty;
        }
        public static string ValidateConfirmPassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return Lang.ErrorPasswordMismatch;
            }
            return string.Empty;
        }
    }
}