using StorageApplication.MVVM.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StorageApplication.MVVM.Helper
{
    static class ValidationClass
    {
        #region Error Message

        public static readonly string EmptyBoxesError = " cannot be empty";
        public static readonly string NotEnoughLettersError = " must have 3 or more letters";
        public static readonly string NotEnoughtDigitError = " must have 10 digits";
        public static readonly string OnlyDigitError = " must have only digits";
        public static readonly string IncorrectPhone = " must be write like example => (+48)456723189";
        public static readonly string NotEnoughLettersCodeError = " must have 1-5 letters";

        public static readonly string PhoneRegexPattern = @"^\(\+\d{1,2}\)\d{9}$";

        #endregion

        public static readonly ObjectPair<int, string> EmptyFKey = new ObjectPair<int, string>(-1, "-- NULL --");

        public static bool IsEmpty(string text) { return string.IsNullOrWhiteSpace(text); }

        #region Text Validate Function

        public static string? CheckOptionalText(string text, string context)
        {
            if (IsEmpty(text)) return null;
            if (text.Length < 3) return (context + NotEnoughLettersError);
            return null;
        }
        public static string? CheckText(string text, string context)
        {
            if (IsEmpty(text)) return (context + EmptyBoxesError);
            if (text.Length < 3) return (context + NotEnoughLettersError);
            return null;
        }

        public static string? CheckTextCode(string text, string context)
        {
            if (IsEmpty(text)) return (context + EmptyBoxesError);
            if (text.Length < 1 || text.Length > 5) return (context + NotEnoughLettersCodeError);
            return null;
        }

        public static string? CheckNIP(string text, string context)
        {
            if (IsEmpty(text)) return (context + EmptyBoxesError);
            if (!text.All(char.IsDigit)) return (context + OnlyDigitError);
            if (text.Length != 10) return (context + NotEnoughtDigitError);
            return null;
        }

        public static string? CheckOptionalPhone(string text, string context)
        {
            if (IsEmpty(text)) return null;
            try
            {
                Regex regex = new Regex(PhoneRegexPattern);
                return regex.IsMatch(text) ? null : (context + IncorrectPhone);
            }
            catch (Exception) { return (context + IncorrectPhone); }
        }
        public static string? CheckPhone(string text, string context)
        {
            if (IsEmpty(text)) return (context + EmptyBoxesError);
            try
            {
                Regex regex = new Regex(PhoneRegexPattern);
                return regex.IsMatch(text) ? null : (context + IncorrectPhone);
            }
            catch (Exception) { return (context + IncorrectPhone); }
        }

        public static string? CheckComboBox(object? value, string context)
        {
            if (value == null) return (context + EmptyBoxesError);
            return null;
        }

        #endregion
    }
}
