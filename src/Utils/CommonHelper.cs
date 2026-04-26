using System;

namespace EasyInvoicePro.Utils
{
    public static class CommonHelper
    {
        public static string FormatCurrency(decimal amount, string currency = "TRY")
        {
            return $"{amount:N2} {currency}";
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input
                .Replace("'", "''")
                .Replace("\\", "\\\\")
                .Trim();
        }

        public static bool IsValidTaxNumber(string taxNumber)
        {
            if (string.IsNullOrWhiteSpace(taxNumber))
                return false;

            if (!taxNumber.All(char.IsDigit))
                return false;

            if (taxNumber.Length != 10)
                return false;

            if (taxNumber[0] == '0')
                return false;

            return true;
        }

        public static bool IsValidIDNumber(string idNumber)
        {
            if (string.IsNullOrWhiteSpace(idNumber))
                return false;

            if (!idNumber.All(char.IsDigit))
                return false;

            if (idNumber.Length != 11)
                return false;

            if (idNumber[0] == '0')
                return false;

            return true;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            var cleanPhone = phone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            if (!cleanPhone.All(char.IsDigit))
                return false;

            return cleanPhone.Length >= 10 && cleanPhone.Length <= 11;
        }

        public static string HtmlEncode(string text)
        {
            return System.Web.HttpUtility.HtmlEncode(text);
        }

        public static string HtmlDecode(string text)
        {
            return System.Web.HttpUtility.HtmlDecode(text);
        }

        public static string CreateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            string slug = text.ToLowerInvariant();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^\w\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[\s]+", "-");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
            slug = slug.Trim('-');

            return slug;
        }

        public static string GetMonthName(int month)
        {
            string[] months = 
            { 
                "Ocak", "Subat", "Mart", "Nisan", "Mayis", "Haziran",
                "Temmuz", "Agustos", "Eylul", "Ekim", "Kasim", "Aralik"
            };

            if (month >= 1 && month <= 12)
                return months[month - 1];

            return "Bilinmiyor";
        }

        public static string GetDayName(DateTime date)
        {
            string[] days = 
            { 
                "Pazar", "Pazartesi", "Sali", "Carsamba", "Persembe", "Cuma", "Cumartesi"
            };

            return days[(int)date.DayOfWeek];
        }
    }
}
