
using E_Commerce_Bot.Persistence;
using E_Commerce_Bot.Services;
using E_Commerce_Bot.Services.Bot;
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
            builder.Services.AddTransient<OrderService>();
            builder.Services.AddTransient<CategoryService>();
            builder.Services.AddTransient<BasketService>();
            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<ProductService>();
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
