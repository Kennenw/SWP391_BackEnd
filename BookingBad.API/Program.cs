
using NETCore.MailKit.Extensions;
using Repositories.DTO;
using Repositories.Repositories;
using Services;
using static Services.EmailServices;

namespace Repositories.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add session support
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add distributed memory cache
            builder.Services.AddDistributedMemoryCache();

            // Add CORS support
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder => builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            // Register EmailService as IEmailSender
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Add MailKit services
            builder.Services.AddMailKit(config => builder.Configuration.GetSection("EmailSettings").Bind(config));

            // Register services
            builder.Services.AddTransient<IEmailServices, EmailServices>();
            builder.Services.AddTransient<IAccountServices, AccountServices>();
            builder.Services.AddTransient<ICourtServices, CourtServices>();

            // Register repositories
            builder.Services.AddScoped<AccountRepo>();
            builder.Services.AddScoped<AccountRepo>();
            builder.Services.AddScoped<CourtRepo>(); 
            builder.Services.AddScoped<SubCourtRepo>();
            builder.Services.AddScoped<AmenityCourtRepo>();
            builder.Services.AddScoped<SlotTimeRepo>();

            // Register memory cache
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Use CORS middleware
            app.UseCors("AllowReactApp");

            app.UseAuthorization();

            // Use session middleware
            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}
