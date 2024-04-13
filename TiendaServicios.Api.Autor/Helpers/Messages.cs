namespace TiendaServicios.Api.Autor.Helpers
{
    public abstract class Messages
    {
        public abstract string book_created { get; }
        public abstract string book_deleted { get; }
        public abstract string author_created { get; }
        public abstract string author_deleted { get; }
    }

    public class EnglishMessages : Messages
    {
        public override string book_created 
        {
            get {
                return "Book created successfully";
            }
        }

        public override string book_deleted
        {
            get
            {
                return "Book deleted successfully";
            }
        }

        public override string author_created
        {
            get
            {
                return "Author created successfully";
            }
        }

        public override string author_deleted
        {
            get
            {
                return "Author deleted successfully";
            }
        }
    }

    public class SpanishMessages : Messages
    {
        public override string book_created
        {
            get
            {
                return "Libro creado correctamente";
            }
        }

        public override string book_deleted
        {
            get
            {
                return "Libro eliminado correctamente";
            }
        }

        public override string author_created
        {
            get
            {
                return "Autor creado correctamente";
            }
        }

        public override string author_deleted
        {
            get
            {
                return "Autor eliminado correctamente";
            }
        }
    }
}
