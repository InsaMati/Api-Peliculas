using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Dto;
using PeliculasAPI.Entidades;
using PeliculasAPI.Helpers;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDBContext context;
        private readonly IMapper mapper;

        public ActoresController(ApplicationDBContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {

            var queryable = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);

            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(entidades);

        }

        [HttpGet("{id:int}", Name = "obtenerActor")]

        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entidades = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (entidades == null) return NotFound();

            return mapper.Map<ActorDTO>(entidades);
        }

        [HttpPost]

        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var entidad = mapper.Map<Actor>(actorCreacionDTO);
            context.Actores.Add(entidad);
            await context.SaveChangesAsync();
            var ActorDTO = mapper.Map<ActorDTO>(entidad);

            return new CreatedAtRouteResult("obtenerActor", new { id = ActorDTO.Id, }, ActorDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actorCreacionDTO)
        {
            var existe = await context.Actores.AnyAsync(context => context.Id == id);

            if (!existe) return NotFound();

            var entidad = mapper.Map<Actor>(actorCreacionDTO);
            entidad.Id = id;

            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existeActor = await context.Actores.AnyAsync(x => x.Id == id);

            if (!existeActor) return NotFound();

            context.Remove(new Actor
            {
                Id = id
            });

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]

        public async Task<ActionResult> Patch (int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if(entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = mapper.Map<ActorPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDTO, entidadDB);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
