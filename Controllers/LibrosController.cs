using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoLibros.Contexts;
using ProyectoLibros.Entities;
using ProyectoLibros.Models;

namespace ProyectoLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Libros
        [HttpGet(Name = "ObtenerLibros")]
        public async Task<ActionResult<IEnumerable<Libro>>> ObtenerLibros()
        {
            return await context.Libros.Include(x => x.Autor).ToListAsync();
        }

        // GET: api/Libros/5
        [HttpGet("{id}", Name = "ObtenerLibro")]
        public async Task<ActionResult<Libro>> ObtenerLibro(int id)
        {
            var libro = await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        // POST: api/Libros
        [HttpPost(Name = "CrearLibro")]
        public async Task<ActionResult> Post([FromBody] Libro libro)
        {
            await context.Libros.AddAsync(libro);
            await context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObtenerLibro", new { id = libro.Id }, libro);
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
