using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Dto;
using PeliculasAPI.Entidades;
using PeliculasAPI.Servicios;

namespace PeliculasAPI.Controllers
{
    [ApiController]

    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDBContext dBContext;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public PeliculasController(ApplicationDBContext dBContext,
            IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]

        public async Task<ActionResult<List<PeliculasDTO>>> Get()
        {
            var peliculas = await dBContext.Peliculas.ToListAsync();
            return mapper.Map<List<PeliculasDTO>>(peliculas);
        }

        [HttpGet("{id:int}", Name = "ObtenerPelicula")]

        public async Task<ActionResult<PeliculasDTO>> Get (int id)
        {
            var entidad = await dBContext.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if(entidad == null)
            {
                return NotFound();
            }

            return mapper.Map<PeliculasDTO>(entidad);
        } 

        [HttpPost]

        public async Task<ActionResult> Post ([FromForm]PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var entidad = mapper.Map<Pelicula>(peliculaCreacionDTO);
            dBContext.Peliculas.Add(entidad);
            await dBContext.SaveChangesAsync();

            var peliculaDTO = mapper.Map<PeliculasDTO>(entidad);

            return new CreatedAtRouteResult ("ObtenerPelicula", new {id = peliculaDTO.Id}, peliculaDTO);
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put ([FromForm]PeliculaCreacionDTO peliculaCreacionDTO, int id)
        {
            var existe = await dBContext.Actores.AnyAsync(context => context.Id == id);

            if (!existe) return NotFound();

            var entidad = mapper.Map<Pelicula>(peliculaCreacionDTO);
            entidad.Id = id;

            dBContext.Entry(entidad).State = EntityState.Modified;
            await dBContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]

        public async Task<ActionResult> Patch (int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await dBContext.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = mapper.Map<PeliculaPatchDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDTO, entidadDB);

            await dBContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existeActor = await dBContext.Peliculas.AnyAsync(x => x.Id == id);

            if (!existeActor) return NotFound();

            dBContext.Remove(new Actor
            {
                Id = id
            });

            await dBContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
