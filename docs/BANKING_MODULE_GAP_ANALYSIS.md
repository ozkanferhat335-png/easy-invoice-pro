# Banka API Entegre Muhasebe Uygulaması - Gap Analizi ve Tamamlama Planı

## Mevcut Durum Özeti
Bu repo şu an ağırlıklı olarak e-fatura/faturalama çekirdeği içeriyor. Banka API, EFT/Havale, döviz dönüşümü ve mutabakat modülleri henüz tam değil.

## FR Kapsam Durumu (Özet)
- **Hazır/Temel:** Kullanıcı ve temel veri modelleme yaklaşımı, servis katmanı paterni.
- **Eksik:** FR-05..FR-31 kapsamının büyük bölümü (banka bağlantısı, hareket senkronizasyonu, transfer yaşam döngüsü, mutabakat, audit derinliği).

## Öncelikli Eksik Paketler
1. **Banking Domain Modeli**
   - BankConnection, BankAccount, BankTransaction, TransferOrder, ExchangeRate, AuditLog entity’leri
2. **Adapter Mimarisi**
   - IBankAdapter + banka bazlı implementasyonlar (Akbank, Garanti vb.)
3. **Güvenlik**
   - Secret/token şifreleme
   - Hassas alan maskeleme
4. **İş Süreci**
   - Duplicate transaction kontrolü (unique hash)
   - Maker-checker akışı
   - Idempotency key ile tekrar EFT engelleme
5. **Muhasebe Entegrasyonu**
   - Transaction -> voucher öneri motoru
6. **Raporlama ve Mutabakat**
   - Günlük/aylık mutabakat
   - Kur farkı ve nakit akış raporu

## Sprint Bazlı Tamamlama Önerisi
### Sprint 1 (Temel Altyapı)
- Domain entity + enumlar
- Bank adapter kontratları
- Banka bağlantı yönetimi

### Sprint 2 (Hareket ve Transfer)
- Hesap hareketi çekme/saklama
- EFT/Havale oluşturma + durum takibi
- Retry/backoff + timeout konfigürasyonu

### Sprint 3 (Muhasebe ve Rapor)
- Otomatik fiş önerileri
- Mutabakat ekranı
- Excel/PDF dışa aktarma

### Sprint 4 (Güvenlik & Uyum)
- Audit trail genişletme
- KVKK maskeleme/erişim politikaları
- Performans testleri ve kabul testleri

## Bu commit ile eklenenler
- SRS dokümanı proje içine alındı.
- Bankacılık modülü için başlangıç seviyesinde domain, enum ve servis kontratları eklendi.
- Böylece geliştirme backlog’u ve kod iskeleti netleştirildi.
