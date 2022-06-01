﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Dto
{
    public class GeneroDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
    }
}
