# 🎬 FilmApp

FilmApp, film arşivi tutmak ve izlenen filmleri kaydetmek için geliştirilmiş bir ASP.NET Core Web API uygulamasıdır.

## 🚀 Özellikler

- 🔐 JWT ile kimlik doğrulama  
- 📄 Swagger UI üzerinden API testleri  
- 🎥 Film ekleme, silme, güncelleme  
- 👤 Kullanıcı kaydı ve girişi  
- ⏱ İzleme geçmişi kaydı  
- 🎭 Tür (genre) sınıflandırması  

## 🛠 Teknolojiler

- ASP.NET Core 7 Web API  
- Entity Framework Core  
- SQL Server (EF Migrations)  
- Swagger / Swashbuckle  
- JWT Authentication  
- xUnit (unit testler)  

## 📦 Proje Yapısı

FilmApp.API -> API controller'ları ve JWT
FilmApp.Business -> Servisler ve arayüzler
FilmApp.DataAccess -> DbContext, Migrations
FilmApp.Entities -> Model sınıfları
FilmApp.Tests -> Unit testler

## 🔧 Nasıl Çalıştırılır?

1. Projeyi klonlayın:

```bash
git clone https://github.com/yagmureryilmazz/FilmApp.git

2. Paketleri geri yükleyin:

dotnet restore

3. Migration'ı uygulayın:

dotnet ef database update

4. Uygulamayı başlatın:

dotnet run --project FilmApp.API

5. Tarayıcıda Swagger UI’yi açarak API'yi test edin:

http://localhost:5236/swagger

🧪 Testler

Unit testleri çalıştırmak için:

dotnet test


Bunu `.md` uzantılı bir dosyaya yapıştırarak GitHub'a gönderebilirsin.

