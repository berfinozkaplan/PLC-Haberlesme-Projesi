# PLC Haberleşme ve Kontrol Arayüzü

Bu proje, zorunlu yaz stajım kapsamında geliştirdiğim, PLC cihazları ile haberleşerek veri okuma ve yazma işlemlerini gerçekleştiren bir masaüstü uygulamasıdır.

## 🎯 Proje Hakkında
Bu proje, Trakya Üniversitesi bünyesindeki eğitim süreci ve Eurasia Robotic staj programı kapsamında geliştirilmiş bir masaüstü otomasyon kontrol arayüzüdür.
Proje, endüstriyel kontrol sistemlerinde kullanılan kullanıcı yetkilendirme süreçlerini ve PLC (Programmable Logic Controller) veri haberleşmesini simüle etmek amacıyla tasarlanmıştır. Visual Studio (Windows Forms) arayüzü ile Microsoft SQL Server (SSMS) veritabanı arasında dinamik bir veri akışı sağlayarak, sahadaki cihazların kontrolünü dijital bir panel üzerinden yönetmeyi hedefler.

## 🚀 Özellikler
* **Güvenli Yetkilendirme:** Kullanıcı adı ve şifre ile giriş yapma, yeni personel kaydı oluşturma ve e-posta doğrulama kodu ile şifre kurtarma desteği.
* **Cihaz Bağlantı Yönetimi:** Hedef PLC veya sunucu için IP adresi ve Port numarasını arayüz üzerinden dinamik olarak yapılandırma ve bağlantı sağlama.
* **Dijital Sinyal Kontrolü:** Seçilen donanımlara (Işık, Motor, Fan, Acil Durum) "True/False" (Aç/Kapa) komutlarını göndererek durum yönetimi.
* **Analog Veri İşleme:** Sistem seviyelerini (En Düşük, Orta, En Yüksek) sayısal değer olarak yazma ve mevcut kayıtlı değerleri veritabanından okuma.
* **Veritabanı Entegrasyonu:** Tüm kullanıcı hareketlerini ve gönderilen komut sinyallerini SQL Server üzerinde loglama ve geçmiş takibi.

## 🛠️ Kullanılan Teknolojiler
* **Dili:** C# (.NET Framework)
* **IDE:** Visual Studio
* **Mimari:** Windows Forms Application & ADO.NET
* **Veritabanı:** Microsoft SQL Server (SSMS)

## 📸 Ekran Görüntüleri
<img width="413" height="511" alt="kullanıcıgirişi" src="https://github.com/user-attachments/assets/65fedcf2-b019-4657-a669-4a9252fb76ab" />
<img width="435" height="305" alt="şifremiunuttum" src="https://github.com/user-attachments/assets/f556341c-0904-4d4e-a3d8-18bb6baeb033" />
<img width="329" height="410" alt="kayıtol" src="https://github.com/user-attachments/assets/b46f20de-9427-4641-a70a-be2ca7ec9dbc" />
<img width="327" height="245" alt="ipadres" src="https://github.com/user-attachments/assets/357dd3d8-f952-4e99-b384-7e9e2b84573c" />
<img width="596" height="497" alt="offset" src="https://github.com/user-attachments/assets/0fe5861f-c728-4ddb-b3f7-0911e1acee75" />
<img width="443" height="402" alt="offsetsayı" src="https://github.com/user-attachments/assets/356016a0-480f-4758-b308-cf23f44af690" />
<img width="1133" height="574" alt="ssms" src="https://github.com/user-attachments/assets/075d5478-e80f-4cfb-bc5a-0825d0cead9d" />

---
*Geliştirici: Berfin Özkaplan*
