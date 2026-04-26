# 📦 EasyInvoice Pro - Kurulum Rehberi

## Sistem Gereksinimleri

### Minimum Gereksinimler
- **İşletim Sistemi:** Windows 7 SP1 veya üstü
- **.NET Framework:** 4.7.2 veya üstü
- **RAM:** 2 GB
- **Disk:** 500 MB boş alan
- **Ekran:** 1024x768 minimum

### Önerilen Spesifikasyonlar
- **İşletim Sistemi:** Windows 10/11
- **.NET Framework:** 4.8
- **RAM:** 4 GB veya üstü
- **Disk:** SSD 1 GB
- **Ekran:** 1280x720 veya üstü

## Adım 1: .NET Framework Yükleme

.NET Framework 4.7.2 yüklü değilse:
1. https://dotnet.microsoft.com/download/dotnet-framework adresine gidin
2. .NET Framework 4.7.2 indirin
3. Kurulum dosyasını çalıştırın
4. Bilgisayarı yeniden başlatın

## Adım 2: EasyInvoice Pro İndirilmesi

### Seçenek A: GitHub'dan Klonlama
```bash
git clone https://github.com/ozkanferhat335-png/easy-invoice-pro.git
cd easy-invoice-pro
```

### Seçenek B: Installer Kullanarak
Latest Release sayfasından `EasyInvoicePro-Setup.exe` indirin

## Adım 3: Geliştirme Ortamında Derleme

### Visual Studio'da
1. `EasyInvoicePro.sln` dosyasını açın
2. Çözümü sağ tıklayın → "Restore NuGet Packages"
3. Build menüsü → "Build Solution" (Ctrl+Shift+B)
4. F5 ile çalıştırın

### Command Line'dan
```bash
dotnet restore
dotnet build
dotnet run
```

## Adım 4: İlk Başlatma

1. Uygulama açılır
2. "Demo Başlat" butonuna tıklayın (30 gün ücretsiz)
3. Şirket bilgilerini girin
4. Sertifika yükleyin (e-Fatura için)
5. Demo veriler otomatik yüklenir

## Adım 5: E-Fatura Ayarları (Opsiyonel)

### Test Ortamında Test Edin
1. Ayarlar → E-Fatura
2. Sertifika: Test sertifikasını yükleyin
3. Ortam: "Test" seçin
4. "Bağlantı Test Et" tıklayın

### Üretim Ortamına Geçiş
1. Gerçek sertifikayı yükleyin
2. Ortam: "Üretim" seçin
3. Herhangi bir fatura göndermeden önce test edin

## Adım 6: Lisans Aktivasyonu

### Demo Süresi Sonrası
1. Ayarlar → Lisans
2. Lisans Anahtarını girin
3. "Aktivasyon" butonuna tıklayın
4. İnternet bağlantısı gereklidir

## 🔧 Sorun Giderme

### Problem: "Framework not found" hatası
**Çözüm:** .NET Framework 4.7.2'yi yeniden yükleyin

### Problem: "Database locked" hatası
**Çözüm:** Diğer EasyInvoice Pro pencerelerini kapatın

### Problem: E-Fatura bağlantısı başarısız
**Çözüm:**
- İnternet bağlantısını kontrol edin
- Firewall ayarlarını kontrol edin
- Test ortamında test edin

### Problem: Uygulama açılmıyor
**Çözüm:**
1. C:\Users\[YourUser]\AppData\Local\EasyInvoicePro klasörünü silin
2. Uygulamayı yeniden başlatın

## 📞 Destek

Herhangi bir sorununuz varsa:
- GitHub Issues: https://github.com/ozkanferhat335-png/easy-invoice-pro/issues
- Email: support@easyinvoicepro.com

---

**Kurulum tamamlandı! Şimdi [Kullanıcı Kılavuzu](KULLANICI_KILAVUZU.md) okuyabilirsiniz.**