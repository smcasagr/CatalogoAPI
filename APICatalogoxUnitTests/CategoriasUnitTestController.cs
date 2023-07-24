using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Logging;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace APICatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository;
        private ILogger<CategoriasController> logger;
        private IConfiguration config;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString =
           "Host=localhost;Port=5432;Pooling=True;Database=CatalogoDB;Username=postgres;Password=admin;";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
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

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            repository = new UnitOfWork(context);

            config = new ConfigurationBuilder().Build();

            var logger = new CustomLoggerProvider(
                new CustomLoggerProviderConfig
                {
                    LogLevel = LogLevel.Information
                });
        }

        #region Testes Unitários

        // Inicio dos testes : método GET
        [Fact]
        public async Task GetCategorias_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper, config, logger);
            var model = new CategoriasParameters()
            {
                PageNumber = 1,
                PageSize = 10
            };

            //Act  
            var data = await controller.GetAll(model);

            //Assert  
            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }

        #endregion
    }
}
