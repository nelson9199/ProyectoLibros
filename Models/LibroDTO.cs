using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoLibros.Models
{
    public class LibroDTO
    {
        [Required]
        public string Titulo { get; set; }
        [Required]
        public int AutorId { get; set; }
        //public AutorDTO Autor { get; set; }
    }
}
