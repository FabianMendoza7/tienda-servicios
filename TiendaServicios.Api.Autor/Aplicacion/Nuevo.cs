using FluentValidation;
using MediatR;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Nuevo
    {
        // FEMO: siguiendo el patrón CQRS tendremos dos clases:
        // Autor: para recibir parámetros desde el controlador.
        // Nuevo: para crear un nuevo autor.

        public class Autor : IRequest
        {
            public required string Nombre { get; set; }
            public required string Apellido { get; set; }
            public DateTime FechaNacimiento { get; set;}
        }

        public class EjecutaValidation: AbstractValidator<Autor>
        {
            public EjecutaValidation()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Autor>
        {
            public readonly ContextoAutor _contexto;

            public Manejador(ContextoAutor contexto)
            {
                _contexto = contexto;
            }

            public async Task Handle(Autor request, CancellationToken cancellationToken)
            {
                var autorLibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = DateTime.SpecifyKind(request.FechaNacimiento, DateTimeKind.Utc),
                    AutorLibroGuid = Guid.NewGuid().ToString()
                };

                _contexto.AutorLibro.Add(autorLibro);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return;
                }

                throw new Exception("No se pudo insertar el autor del libro");
            }
        }
    }
}
