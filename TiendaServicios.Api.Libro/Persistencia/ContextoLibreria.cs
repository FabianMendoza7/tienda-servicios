using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria() { }

        // FEMO: permite setear la cadena de conexión a la BD Sql Server desde la clase program.
        public ContextoLibreria(DbContextOptions<ContextoLibreria> options): base(options) { }

        // FEMO: hacer que la clase LibroMaterial se convierta en una clase de tipo entidad.
        // permitiendo por ejemplo migrarla a la BD.
        // Se marca como vistual para que pueda sobreeescribirse a futuro.
        public virtual DbSet<LibroMaterial> LibroMaterial { get; set; }
    }
}
