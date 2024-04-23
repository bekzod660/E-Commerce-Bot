
using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Helpers;
using E_Commerce_Bot.Persistence;
using E_Commerce_Bot.Persistence.Repositories;
using E_Commerce_Bot.Services;
using E_Commerce_Bot.Services.Bot;
using E_Commerce_Bot.Services.Bot.Handlers;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace E_Commerce_Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient("ecommerce")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {


                    return new TelegramBotClient(builder.Configuration.GetConnectionString("Bot"), httpClient);
                });
            builder.Services.AddHostedService<Bot>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"),
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            }, ServiceLifetime.Singleton);
            builder.Services.AddTransient<IUpdateHandler, UpdateHandler>();
            builder.Services.AddTransient<OrderHandler>();
            builder.Services.AddTransient<SettingsHandler>();
            builder.Services.AddTransient<BasketHandler>();
            builder.Services.AddTransient<BackHandler>();
            builder.Services.AddTransient<IUpdateHandler, UpdateHandler>();
            builder.Services.AddTransient<IBotResponseService, BotResponseService>();
            builder.Services.AddTransient<ILocalizationHandler, LocalizationHandler>();
            builder.Services.AddTransient<IBaseRepository<Category>, CategoryRepository>();
            builder.Services.AddTransient<IBaseRepository<Product>, ProductRepository>();
            builder.Services.AddTransient<IBaseRepository<User>, UserRepository>();
            builder.Services.AddMemoryCache();
            builder.Services.AddTransient<TokenService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
