﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dto
{
    public class PeliculasDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        public bool enCines { get; set; }

        public DateTime FechaEstreno { get; set; }

        public string Poster { get; set; }
    }
}
