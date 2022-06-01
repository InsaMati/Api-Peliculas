using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Dto;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GeneroController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public GeneroController(ApplicationDBContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await context.Generos.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(entidades);

            return dtos;
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var entidades = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if(entidades == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<GeneroDTO>(entidades);

            return dto;
        }

        [HttpPost]

        public async Task<ActionResult> Post([FromBody]GeneroCreacionDTO creacionGenero)
        {
            var entidad = mapper.Map<Genero>(creacionGenero);
            context.Generos.Add(entidad);
            await context.SaveChangesAsync();
            var generoDTO = mapper.Map<GeneroDTO>(entidad);

            return new CreatedAtRouteResult("obtenerGenero", new { id = generoDTO.Id }, generoDTO);
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe) return NotFound();

            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;

            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe) return NotFound();

            context.Remove(new Genero()
            {
                Id = id
            });

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
