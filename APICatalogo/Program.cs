
using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace APICatalogo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);            

            // Add services to the container.
            // Solu��o para o problema de serializa��o c�clica de JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions
                        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var dbConn = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dbConn)
            );

            builder.Services.AddTransient<IMeuServico, MeuServico>(); // criado a cada request

            builder.Services.AddScoped<ApiLoggingFilter>(); // inserindo o servi�o de filtro customizado

            // Adiciona o logger caso seja ambiente de desenvolvimento
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.AddProvider(new CustomLoggerProvider(
                    new CustomLoggerProviderConfig
                    {
                        LogLevel = LogLevel.Information
                    }));
            }

            // Registrando servi�o do Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Registrando o servi�o o AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            var app = builder.Build();

            // Adicionando o middleware de tratamento de erro
            app.ConfigureExceptionHandler();

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