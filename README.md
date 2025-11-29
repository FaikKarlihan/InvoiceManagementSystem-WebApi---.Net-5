# Invoice Management System (IMS) - Web API

![.NET Core](https://img.shields.io/badge/.NET%205-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)

Bu proje, site/apartman yönetimleri için geliştirilmiş; fatura takibi, aidat ödemeleri ve kullanıcı yönetimini sağlayan kapsamlı bir **Fatura Yönetim Sistemi** (Invoice Management System) backend projesidir. Proje, **Domain Driven Design (DDD)** prensiplerinden ilham alınarak katmanlı mimari ile geliştirilmiştir.

Ek olarak, bankacılık işlemlerini simüle eden ve **MongoDB** tabanlı çalışan ayrı bir **Payment API** servisi içermektedir.

## 🚀 Özellikler

### 1. Temel İşlevler (Core API)
* **Kullanıcı & Daire Yönetimi:** Kullanıcılar ve daireler için tam CRUD işlemleri.
* **Fatura Yönetimi:** Faturalar için ekleme, silme, güncelleme ve listeleme (CRUD).
* **Admin Yetkileri:**
    * Dairelere kullanıcı atama.
    * Kullanıcılara fatura/aidat atama.
* **Kullanıcı Yetkileri:**
    * Yöneticiye mesaj gönderme.
    * Kredi kartı ile ödeme yapma.
    * Fatura ve ödeme geçmişini görüntüleme.
    * Parola değiştirme (İlk kayıt anında sistem otomatik parola üretir ve DB'de **hash**lenmiş olarak saklar).

### 2. Mimari ve Teknoloji Yığını
* **Framework:** .NET 5 Web API
* **Veri Tabanı (İlişkisel):** PostgreSQL (Entity Framework Core)
* **Mimari:** DDD (Domain Driven Design) tabanlı katmanlı yapı.
* **Güvenlik:** JWT (JSON Web Token) ve Claim bazlı kimlik doğrulama/yetkilendirme.
* **Mapping:** AutoMapper ile nesne dönüşümleri.
* **Validasyon:** FluentValidation ile veri tutarlılığı ve güvenliği.
* **Loglama & Takip:** İşlem sonuçlarının detaylı takibi.

### 3. Ödeme Sistemi (Payment API - Microservice)
Kullanıcıların ödeme yapabilmesi için banka simülasyonu yapan harici bir servistir.
* **Veri Tabanı:** MongoDB
* **İşleyiş:** İki aşamalı güvenli ödeme (2-Phase Commit Simulation):
    1.  **Initiate Payment:** Ödeme başlatılır, geçerli ise bir `payment token` döner.
    2.  **Confirm Payment:** Token ile ödeme kesinleştirilir.
* **Kayıt:** Başarılı ve başarısız tüm işlem istekleri loglanır.

---

## 🛠 Kurulum ve Çalıştırma

Proje hem **Docker** üzerinde containerize edilerek hem de **Local** ortamda çalıştırılabilir.

### Seçenek 1: Docker ile Çalıştırma (Önerilen)

Proje tam dockerize haldedir. PostgreSQL ve MongoDB container'ları dahil tüm sistemi ayağa kaldırmak için:

1.  Repoyu klonlayın:
    ```bash
    git clone https://github.com/FaikKarlihan/InvoiceManagementSystem-WebApi---.Net-5.git
    cd InvoiceManagementSystem-WebApi---.Net-5
    ```

2.  `.env` dosyasını oluşturun:
    Proje kök dizininde `.env.example` dosyasını kopyalayarak `.env` adında bir dosya oluşturun ve gerekli değişkenleri tanımlayın.

    **Örnek `.env` içeriği:**
    ```env
    # Database Settings
    POSTGRES_USER=admin
    POSTGRES_PASSWORD=password123
    POSTGRES_DB=InvoiceDb

    # Mongo Settings
    MONGO_INITDB_ROOT_USERNAME=admin
    MONGO_INITDB_ROOT_PASSWORD=password123
    
    # App Settings
    JWT_KEY=Bu_Cok_Gizli_Bir_Anahtardir_Lutfen_Degistirin_123
    ```

3.  Container'ları ayağa kaldırın:
    ```bash
    docker-compose up -d --build
    ```

### Seçenek 2: Local Ortamda Çalıştırma

Docker kullanmadan IDE (Visual Studio / VS Code) üzerinden çalıştırmak isterseniz:

**Web API (Ana Servis):**
* PostgreSQL bağlantı stringi `appsettings.json` içerisinde tanımlı değilse veya boş geçilirse, uygulama otomatik olarak **In-Memory Database** modunda çalışarak test edilmeye olanak tanır.
* *Not: Prodüksiyon benzeri test için PostgreSQL connection string girilmelidir.*

**Payment API:**
* Bu servisin çalışması için çalışan bir **MongoDB** bağlantısı (Local veya Cloud) zorunludur. Connection string `appsettings.json` içerisine girilmelidir.

---

## 🔌 Port ve Erişim Bilgileri

Uygulama yapılandırması gereği hem **Local** çalışma ortamında hem de **Docker** konteynerlerinde dışarıya açılan portlar birbiriyle eşleşecek şekilde ayarlanmıştır. Uygulama ayağa kalktığında aşağıdaki adreslerden erişebilirsiniz:

| Servis | Protokol | Erişim Adresi (Swagger) | Docker İç Port |
| :--- | :--- | :--- | :--- |
| **Web API** | HTTPS | `https://localhost:5001/swagger` | 443 |
| **Web API** | HTTP | `http://localhost:5000/swagger` | 80 |
| **Payment API** | HTTPS | `https://localhost:5003/swagger` | 443 |
| **Payment API** | HTTP | `http://localhost:5002/swagger` | 80 |

**Not:** Containerlar arası iletişimde Web API ve Payment API birbirlerine internal network üzerinden 80 portundan ulaşmaktadır (`http://web_api:80` ve `http://payment_api:80`).

*(Not: Portlar `docker-compose.yml` veya `launchSettings.json` konfigürasyonunuza göre farklılık gösterebilir, lütfen ilgili dosyaları kontrol ediniz.)*

---

## 🧪 Test Kullanıcıları ve Senaryolar

Sistem ilk ayağa kalktığında veritabanı boş ise (veya In-Memory modda) Seed Data çalışabilir (koda bağlıdır). Manuel test için:

1.  **Admin Girişi:** `/api/Auth/Login` (Admin yetkisine sahip kullanıcı seed data ile eklenir mail:a password:123456).
2.  **Daire/Kullanıcı Ekleme:** Admin token'ı ile Header'da `Authorization: Bearer <token>` kullanarak işlem yapın.
3.  **Ödeme:** Payment API üzerinden kredi kartı bakiyesi tanımlayın, ardından Web API üzerinden ödeme isteği gönderin.

---

## 📝 Lisans

Bu proje MIT lisansı ile lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakabilirsiniz.
