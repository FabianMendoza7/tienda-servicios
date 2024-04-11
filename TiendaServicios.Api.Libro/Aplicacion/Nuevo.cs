using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        //public class Ejecuta: IRequest
        public class Libro : IRequest
        {
            public string? Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion: AbstractValidator<Libro>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("Por favor, proporciona un título para el libro.");
                RuleFor(x => x.FechaPublicacion).NotEmpty().WithMessage("La fecha de publicación es obligatoria.")
                    .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de publicación no puede ser en el futuro.");
                RuleFor(x => x.AutorLibro).NotEmpty().WithMessage("Por favor, proporciona la identificación del autor del libro.");
            }
        }

        public class Manejador : IRequestHandler<Libro>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task Handle(Libro request, CancellationToken cancellationToken)
            {
                var libro = new LibroMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };

                _contexto.LibroMaterial.Add(libro);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return;
                }

                throw new Exception("No se pudo guardar el libro");
            }
        }
    }
}
