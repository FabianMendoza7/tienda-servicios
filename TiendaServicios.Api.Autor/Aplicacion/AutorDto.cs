using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class AutorDto
    {
        // FEMO: El objetivo de este DTO es modelar la data que se va a enviar al cliente.
        // Utilizado solo para las consultas.
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public ICollection<GradoAcademico>? ListaGradoAcademico { get; set; }
        public required string AutorLibroGuid { get; set; }
    }
}
