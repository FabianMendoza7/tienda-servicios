using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Consulta
    {
        public class ListaLibro : IRequest<List<LibroMaterialDto>> 
        { 
        }

        public class Manejador : IRequestHandler<ListaLibro, List<LibroMaterialDto>>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<LibroMaterialDto>> Handle(ListaLibro request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibroMaterial.ToListAsync();
                var librosDto = _mapper.Map<List<LibroMaterial>, List<LibroMaterialDto>>(libros);

                return librosDto;
            }
        }
    }
}
