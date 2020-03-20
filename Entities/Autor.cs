﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoLibros.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
