using PeliculasAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dto
{
    public class ActorCreacionDTO : ActorPatchDTO
    {

        [PesoArchivoValidacion(pesoMaximoenMB:4)]
        [TipoArchivoValidacion(grupoTipoArchivo:GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
