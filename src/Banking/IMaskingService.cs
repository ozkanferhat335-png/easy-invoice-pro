namespace EasyInvoicePro.Banking
{
    public interface IMaskingService
    {
        string MaskIban(string iban);
        string MaskText(string value, int visibleStart = 2, int visibleEnd = 2);
    }
}
