namespace TiendaServicios.Api.Autor.Modelo
{
    public class GradoAcademico
    {
        public int GradoAcademicoId { get; set; }
        public required string Nombre { get; set; }
        public required string CentroAcademico { get; set; }
        public DateTime FechaGrado { get; set; }
        public int AutorLibroId { get; set; }
        public AutorLibro AutorLibro {  get; set; }
        public required string GradoAcademicoGuid { get; set; }
    }
}
