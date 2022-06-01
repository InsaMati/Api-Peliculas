using AutoMapper;
using PeliculasAPI.Controllers;
using PeliculasAPI.Dto;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>();
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculasDTO>().ReverseMap();   
            CreateMap<PeliculaCreacionDTO, Pelicula>();
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }
    }
}
