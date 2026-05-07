using System;
using System.Linq;

namespace EasyInvoicePro.Banking
{
    public class MaskingService : IMaskingService
    {
        public string MaskIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban)) return iban;
            var chars = new string(iban.Where(char.IsLetterOrDigit).ToArray());
            return MaskText(chars, 4, 4);
        }

        public string MaskText(string value, int visibleStart = 2, int visibleEnd = 2)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length <= visibleStart + visibleEnd) return new string('*', value.Length);

            var middle = new string('*', value.Length - (visibleStart + visibleEnd));
            return value.Substring(0, visibleStart) + middle + value.Substring(value.Length - visibleEnd);
        }
    }
}
