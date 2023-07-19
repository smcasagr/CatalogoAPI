
using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Habilitando o Swagger para usar autenticação JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "APICatalogo",
                    Description = "Catálogo de Produtos e Categorias",
                    TermsOfService = new Uri("https://nonexiste.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Samuel",
                        Email = "firkraag@gmail.com",
                        Url = new Uri("https://nonexiste.net/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usar sobre LICX",
                        Url = new Uri("https://nonexiste.net/license"),
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\n" +
                    "Informe 'Bearer'[espaço] eo seu toke.\r\n\r\nExemplo: \'Bearer 12345qwerty\'",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            var dbConn = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dbConn)
            );

            builder.Services.AddTransient<IMeuServico, MeuServico>(); // criado a cada request

            builder.Services.AddScoped<ApiLoggingFilter>(); // inserindo o serviço de filtro customizado

            // Adiciona o logger caso seja ambiente de desenvolvimento
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.AddProvider(new CustomLoggerProvider(
                    new CustomLoggerProviderConfig
                    {
                        LogLevel = LogLevel.Information
                    }));
            }

            // Adiciona o Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();

            // JWT
            // adiciona o hadler de autenticação e define o
            // esquema de autenticação usado Bearer
            // valida o emissor, a audiência e a chave
            // usando a chave secreta, valida a assinatura
            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
                        ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    });

            // Adicionando configuração de versionamento
            builder.Services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            // Registrando serviço do Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Registrando o serviço o AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

/*
            // Configurando limitação CORS via serviço
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("PermitirApiRequest",
                                builder => builder.WithOrigins("https://apirequest.io")
                                                .WithMethods("GET"));
            });*/
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", c =>
                {
                    c.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Adicionando o middleware de tratamento de erro
            app.ConfigureExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Habilitando o middleware do Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

/*
            // Limitando CORS via middleware
            app.UseCors(opt =>
            {   
                opt.WithOrigins("https://apirequest.io")
                    .WithMethods("GET";
            });*/   
            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}