﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoLibros.Models
{
    public class AutorDTO : Recurso
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public List<LibroDTO> Libros { get; set; }
    }
}
