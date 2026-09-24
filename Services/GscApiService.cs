using Google.Apis.Auth.OAuth2;
using Google.Apis.SearchConsole.v1;
using Google.Apis.SearchConsole.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KurumsalWebSitesi.Services
{
    public class GscApiService
    {
        public async Task<SearchAnalyticsQueryResponse> GetSeoVerileriAsync(string propertyUrl, string jsonKeyPath)
        {
            // Ayarların eksiksiz girildiğinden ve JSON dosyasının sunucuda fiziksel olarak var olduğundan emin oluyoruz
            if (string.IsNullOrEmpty(propertyUrl) || string.IsNullOrEmpty(jsonKeyPath) || !File.Exists(jsonKeyPath))
            {
                return null;
            }

            try
            {
                // 1. Google API Kimlik Doğrulaması
                GoogleCredential credential;
                using (var stream = new FileStream(jsonKeyPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(SearchConsoleService.Scope.WebmastersReadonly);
                }

                // 2. Search Console Servisini Başlatma
                var service = new SearchConsoleService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "KurumsalWebSitesi GSC Entegrasyonu"
                });

                // 3. Sorgu Tarihlerini Belirleme (Son 30 günün verisi)
                string startDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");

                // 4. API İstek Gövdesini Oluşturma
                var queryRequest = new SearchAnalyticsQueryRequest
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Dimensions = new List<string> { "query" }, // Hangi arama terimlerinden gelindiğini görmek için
                    RowLimit = 10 // En çok trafik getiren ilk 10 arama terimini çeker
                };

                // 5. İsteği Gönder ve Yanıtı Al
                var request = service.Searchanalytics.Query(queryRequest, propertyUrl);
                var response = await request.ExecuteAsync();

                return response;
            }
            catch (Exception)
            {
                // Hatalı JSON dosyası, yetkilendirilmemiş mail adresi veya yanlış mülk URL'si gibi durumlarda
                // uygulamanın hata ekranına düşmemesi için hatayı yakalayıp null döndürüyoruz.
                return null;
            }
        }
    }
}