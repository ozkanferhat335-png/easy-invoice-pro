using System;
using System.Linq;

namespace EasyInvoicePro.Banking
{
    public static class IBANValidator
    {
        public static bool IsValid(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban)) return false;

            var normalized = new string(iban.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();
            if (normalized.Length < 15 || normalized.Length > 34) return false;

            var rearranged = normalized.Substring(4) + normalized.Substring(0, 4);
            var numeric = string.Concat(rearranged.Select(ch => char.IsLetter(ch) ? (ch - 'A' + 10).ToString() : ch.ToString()));

            int remainder = 0;
            foreach (var c in numeric)
            {
                remainder = (remainder * 10 + (c - '0')) % 97;
            }

            return remainder == 1;
        }
    }
}
