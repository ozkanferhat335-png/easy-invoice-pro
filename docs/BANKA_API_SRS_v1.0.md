# Yazılım Gereksinim Şartnamesi (SRS)

## Proje Adı
Banka API Entegre Muhasebe Uygulaması (Windows Forms)

## Sürüm
v1.0 (Taslak)

## Tarih
02 Mayıs 2026

Bu doküman, kullanıcı tarafından paylaşılan gereksinim metninin proje içine resmi SRS olarak alınmış halidir.

## 1. Giriş
### 1.1 Amaç
Bu doküman, bankaların API’lerini kullanarak geliştirilecek masaüstü muhasebe uygulamasının fonksiyonel ve fonksiyonel olmayan gereksinimlerini tanımlar.

### 1.2 Kapsam
- Banka hesaplarının merkezi yönetimi
- Hesap hareketlerinin otomatik/manuel senkronizasyonu
- EFT/Havale talimatı oluşturma ve durum takibi
- Kur bilgisi alma, dövizli işlemlerin baz para birimine çevrilmesi
- Muhasebe fişi önerisi/oluşturulması
- Mutabakat ve raporlama
- Denetim izi (audit trail)

### 1.3 Tanımlar ve Kısaltmalar
- EFT: Elektronik Fon Transferi
- Havale: Aynı banka içi transfer
- API: Application Programming Interface
- IBAN: International Bank Account Number
- KVKK: Kişisel Verilerin Korunması Kanunu
- Maker-Checker: İşlemi oluşturan ve onaylayan kişinin farklı olması kuralı

## 2. Genel Tanım
### 2.1 Ürün Perspektifi
Uygulama Windows üzerinde çalışan bağımsız bir masaüstü uygulamadır (WinForms, .NET Framework/C# 7.3).

### 2.2 Ürün Fonksiyonları
- Kullanıcı/rol yönetimi
- Banka bağlantı yapılandırma
- Hesap hareketleri çekme
- EFT/Havale yönetimi
- Döviz kuru alma ve dönüşüm
- Otomatik fiş öneri/eşleştirme
- Raporlama
- Audit log

## 3. Fonksiyonel Gereksinimler
FR-01..FR-31 maddeleri birebir uygulanacaktır (tam liste iş paketlerine dağıtılmıştır).

## 4. Harici Arayüz Gereksinimleri
- UI: WinForms modüler ekranlar
- Yazılım Arayüzleri: Banka REST/SOAP API’leri, SQL Server erişimi, Excel/PDF kütüphaneleri
- İletişim: HTTPS/TLS 1.2+, Proxy/VPN desteği

## 5. Fonksiyonel Olmayan Gereksinimler
NFR-01..NFR-18 güvenlik, performans, güvenilirlik, kullanılabilirlik, bakım ve mevzuat uyumluluğunu kapsar.

## 6. Use Case’ler
- UC-01 Hesap Hareketi Senkronizasyonu
- UC-02 EFT Talimatı Oluşturma ve Onay
- UC-03 Dövizli İşlemin Muhasebeleştirilmesi

## 7. Veri Modeli (Yüksek Seviye)
Önerilen tablolar:
- Users
- Companies
- BankConnections
- BankAccounts
- BankTransactions
- TransferOrders
- ExchangeRates
- AccountingVouchers
- VoucherLines
- AuditLogs

## 8. İş Kuralları
- BR-01: Aynı banka hareketi birden fazla kez muhasebeleştirilemez.
- BR-02: EFT işlemlerinde alıcı IBAN format doğrulaması zorunludur.
- BR-03: Dövizli fişlerde kullanılan kur bilgisi değiştirilemez logla korunur.
- BR-04: Onay gerektiren tutar limiti parametrelenebilir olmalıdır.

## 9. Kabul Kriterleri
AK-01..AK-05 maddeleri doğrulama/test planı olarak uygulanacaktır.

## 10. Kısıtlar ve Riskler
- Banka API versiyon değişiklikleri
- API oran limitleri
- Ağ kesintileri
- Mevzuat değişiklikleri
