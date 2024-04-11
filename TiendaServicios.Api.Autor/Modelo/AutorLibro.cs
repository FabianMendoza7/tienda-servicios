namespace TiendaServicios.Api.Autor.Modelo
{
    public class AutorLibro
    {
        public int AutorLibroId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public ICollection<GradoAcademico>? ListaGradoAcademico { get; set; }
        public required string AutorLibroGuid { get; set; }
    }
}
