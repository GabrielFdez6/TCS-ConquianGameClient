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
        private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(250);
        private const string NamePattern = @"^[a-zA-Z\s]+$";
        private const string LastNamePattern = @"^[a-zA-Z\s]+$";
        private const string NicknamePattern = @"^[a-zA-Z0-9]+$";
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$";
        private const string UppercasePattern = @"[A-Z]";
        private const string SpecialCharPattern = @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]";

        private const int MAX_NAME_LENGTH = 25;
        private const int MAX_LAST_NAME_LENGTH = 50;
        private const int MAX_NICKNAME_LENGTH = 15;
        private const int MAX_EMAIL_LENGTH = 45;
        private const int MIN_PASSWORD_LENGTH = 8;
        private const int MAX_PASSWORD_LENGTH = 15;


        private static bool IsMatchWithTimeout(string input, string pattern)
        {
            bool isMatch;

            try
            {
                isMatch = Regex.IsMatch(input, pattern, RegexOptions.None, RegexTimeout);
            }
            catch (RegexMatchTimeoutException)
            {
                isMatch = false;
            }
            return isMatch;
        }
        public static string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Lang.ErrorNameEmpty;
            }
            name = name.Trim();
            if (name.Length > MAX_NAME_LENGTH)
            {
                return string.Format(Lang.ErrorNameLength, MAX_NAME_LENGTH);
            }
            if (!IsMatchWithTimeout(name, NamePattern))
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

            if (!IsMatchWithTimeout(lastName, LastNamePattern))
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

            if (!IsMatchWithTimeout(nickname, NicknamePattern))
            {
                return Lang.ErrorNicknameInvalidChars;
            }

            return string.Empty;
        }

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Lang.ErrorEmailEmpty;
            }
            email = email.Trim();
            if (email.Length > MAX_EMAIL_LENGTH)
            {
                return string.Format(Lang.ErrorEmailLenght, MAX_EMAIL_LENGTH);
            }

            if (!IsMatchWithTimeout(email, EmailPattern))
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

            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
            {
                return string.Format(Lang.ErrorPasswordLength, MIN_PASSWORD_LENGTH, MAX_PASSWORD_LENGTH);
            }

            if (password.Contains(" "))
            {
                return Lang.ErrorPasswordNoSpaces;
            }

            if (!IsMatchWithTimeout(password, UppercasePattern))
            {
                return Lang.ErrorPasswordNoUppercase;
            }

            if (!IsMatchWithTimeout(password, SpecialCharPattern))
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