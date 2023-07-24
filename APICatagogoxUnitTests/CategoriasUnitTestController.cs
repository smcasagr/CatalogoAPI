using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Logging;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private readonly IUnitOfWork uof;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connString =
            "Host=localhost;Port=5432;Pooling=True;Database=CatalogoDB;Username=postgres;Password=admin;";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connString)
                .Options;
        }

        public CategoriasUnitTestController()
        {
            // Registrando o serviço o AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            // Caso fosse utilizar outro banco de dados
            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);
            uof = new UnitOfWork(context);

            logger = new CustomLoggerProvider(
                new CustomLoggerProviderConfig
                {
                    LogLevel = LogLevel.Information
                });

            configuration = new ConfigurationBuilder().Build();

        }

        #region TestesUnitários

        // Testar método GET
        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(uof, mapper, configuration, logger);

            //Act
            var data = controller.GetAll(null);

            //Assert
            Assert.IsType<Task<List<CategoriaDTO>>>(data);
        }

        #endregion
    }
}
