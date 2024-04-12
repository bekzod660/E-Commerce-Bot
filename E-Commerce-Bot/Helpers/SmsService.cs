using System.Text.Json;

namespace E_Commerce_Bot.Helpers
{
    public class SmsService
    {
        private static string message = "Choyxona : {0}";
        public static async Task<string> GetToken(string email, string password)
        {
            string apiUrl = "http://notify.eskiz.uz/api/auth/login";

            using (HttpClient client = new HttpClient())
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(email), "email");
                formData.Add(new StringContent(password), "password");

                HttpResponseMessage response = await client.PostAsync(apiUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var body = JsonSerializer.Deserialize<AuthBody>(responseBody);

                    return body?.data?.token ?? "";
                }
                return "";
            }
        }

        public static async Task SendSms(string token, string phoneNumber, string code)
        {
            string apiUrl = "http://notify.eskiz.uz/api/message/sms/send";

            using (HttpClient client = new HttpClient())
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(phoneNumber), "mobile_phone");
                formData.Add(new StringContent(string.Format(message, code)), "message");
                formData.Add(new StringContent("4546"), "from");
                formData.Add(new StringContent("http://0000.uz/test.php"), "callback_url");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, formData);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response: " + responseBody);
                }
                else
                {
                    Console.WriteLine("Request failed with status code " + response.StatusCode);
                }
            }
        }
        public class Data
        {
            public string token { get; set; }
        }

        public class AuthBody
        {
            public string message { get; set; }
            public Data data { get; set; }
            public string token_type { get; set; }
        }
    }
}
