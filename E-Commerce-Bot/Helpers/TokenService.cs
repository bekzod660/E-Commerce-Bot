using Microsoft.Extensions.Caching.Memory;

namespace E_Commerce_Bot.Helpers
{
    public class TokenService
    {
        private readonly IMemoryCache cache;
        private readonly IConfiguration configuration;
        private const string SmsToken = "SmSToken";
        public TokenService(IMemoryCache cache, IConfiguration configuration)
        {
            this.cache = cache;
            this.configuration = configuration;
        }

        public async Task<string> GetSmsTokenAsync()
        {
            if (cache.TryGetValue(SmsToken, out string token))
            {
                return token;
            }

            token = await GenerateEskizTokenAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(20));
            cache.Set(SmsToken, token, cacheOptions);

            return token;
        }

        private async Task<string?> GenerateEskizTokenAsync()
        {
            var eskiz = new Eskiz();
            configuration.Bind("Eskiz", eskiz);
            if (eskiz is not { })
                throw new KeyNotFoundException("Eskiz credential not found, please, contact support!");

            return await SmsService.GetToken(eskiz.Email!, eskiz.Password!);
        }
        private class Eskiz
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }
    }
}
