using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibroServiceTest
    {
        private IEnumerable<LibroMaterial> ObtenerDataPrueba()
        {
            // Devuelve datos falsos para pruebas (usa GenFu).
            A.Configure<LibroMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibroMaterialId, () => { return Guid.NewGuid(); });

            // Tener un Id estático para futuras pruebas por dicho Id.
            var lista = A.ListOf<LibroMaterial>(30);
            lista[0].LibroMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            // Indicar que la clase LibroMaterial sea de tipo entidad.
            // para eso hay que configurar todas la propiedades requeridas por EF.
            var dbSet = new Mock<DbSet<LibroMaterial>>();
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            // Como los métodos usados para obtener la data son asíncronos, necesitamos que nuestra entidad tambien
            // tenga esas propiedades asíncronas, por lo tanto debemos agregar dos clases: asyncEnumerator y asyncEnumerable.
            // el 1ro para evaluar el array q devuelve el EF
            dbSet.As<IAsyncEnumerable<LibroMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibroMaterial>(dataPrueba.GetEnumerator()));

            // Esta linea permite hacer filtros.
            dbSet.As<IQueryable<LibroMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibroMaterial>(dataPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibroMaterial).Returns(dbSet.Object);

            return contexto;
        }

        [Fact]
        public async void GetLibroPorId()
        {
            // 1. ARRANGE.
            var mockContexto = CrearContexto();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mockMapper = mapperConfig.CreateMapper();

            var request = new ConsultaFiltro.LibroUnico();
            request.LibroGuid = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mockMapper);

            // 2. ACT.
            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            // 3. ASSERT.
            Assert.NotNull(libro);
            Assert.True(libro.LibroMaterialId == Guid.Empty);
        }

        [Fact]
        public async void GetLibros()
        {
            //System.Diagnostics.Debugger.Launch();

            // 1. ARRANGE.

            // a. Emular la instancia de Entity Framework Core - ContextoLibreria con objetos de tipo Mock

            var mockContexto = CrearContexto();

            // b. Emular al mapping IMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mockMapper = mapperConfig.CreateMapper();   

            // c. Instanciar a la clase Manejador y pasarle como parámetros los mocks que he creado.
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mockMapper);

            Consulta.ListaLibro request = new Consulta.ListaLibro();

            // 2. ACT.
            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            // 3. ASSERT.
            Assert.True(lista.Any());
        }

        [Fact]
        public async void GuardarLibro()
        {
            // 1. ARRANGE.
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;
            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Libro();
            request.Titulo = "Libro de Microservicios";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);

            // 2. ACT.
            await manejador.Handle(request, new System.Threading.CancellationToken());

            // 3. ASSERT.
            try
            {
                // Verificar que el libro se agregó correctamente a la base de datos
                var libroAgregado = await contexto.LibroMaterial.FirstOrDefaultAsync(l => l.Titulo == request.Titulo);

                Assert.NotNull(libroAgregado);
                Assert.Equal(request.Titulo, libroAgregado.Titulo); 
            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción: {ex.Message}");
            }
        }
    }
}
