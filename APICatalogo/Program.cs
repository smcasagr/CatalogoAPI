
using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

namespace APICatalogo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Solução para o problema de serialização cíclica de JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions
                        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddTransient<IMeuServico, MeuServico>(); // criado toda vez que precisar

            builder.Services.AddScoped<ApiLoggingFilter>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var dbConn = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dbConn)
            );            

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