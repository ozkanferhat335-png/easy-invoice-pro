namespace EasyInvoicePro.Banking
{
    public enum UserRole
    {
        Admin = 0,
        Muhasebe = 1,
        Finans = 2,
        Izleyici = 3
    }

    public enum TransferType
    {
        EFT = 0,
        Havale = 1
    }

    public enum TransferStatus
    {
        Beklemede = 0,
        Onaylandi = 1,
        Gonderildi = 2,
        Basarisiz = 3,
        Tamamlandi = 4
    }
}
