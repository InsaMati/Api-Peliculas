using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Controllers
{
    public class PeliculaPatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }

        public bool enCines { get; set; }

        public DateTime FechaEstreno { get; set; }
        [PesoArchivoValidacion(pesoMaximoenMB: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }
    }
}