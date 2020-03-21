using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoLibros.Models
{
    public class HashResult
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
