using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoLibros.Contexts;
using ProyectoLibros.Entities;
using ProyectoLibros.Helpers;
using ProyectoLibros.Models;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace ProyectoLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Autores
        [HttpGet(Name = "ObtenerAutores")]
        [ServiceFilter(typeof(HATEOASAuthorsFilterAttribute))]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get(int numeroDePagina = 1, int cantidadDeRegistros = 10)
        {
            var query = context.Autores.AsQueryable();

            var totalDeRegistros = query.Count();

            var autores = await query
                .Skip(cantidadDeRegistros * (numeroDePagina - 1))
                .Take(cantidadDeRegistros)
                .ToListAsync();

            Response.Headers["X-Total-Registros"] = totalDeRegistros.ToString();
            Response.Headers["X-Cantidad-Paginas"] =
                ((int)Math.Ceiling((double)totalDeRegistros / cantidadDeRegistros)).ToString();

            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);
            return autoresDTO;
        }



        // GET: api/Autores/5
        [HttpGet("{id}", Name = "ObtenerAutor")]
        [ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
        public async Task<ActionResult<AutorDTO>> ObtenerAutor(int id)
        {
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorDto = mapper.Map<AutorDTO>(autor);

            return autorDto;
        }

        private void GenerarEnlaces(AutorDTO autor)
        {
            autor.Enlaces.Add(new Enlace(Url.Link("ObtenerAutor", new { id = autor.Id }), rel: "self", metodo: "GET"));
            autor.Enlaces.Add(new Enlace(Url.Link("ActualizarAutorPut", new { id = autor.Id }), rel: "update-author", metodo: "PUT"));
            autor.Enlaces.Add(new Enlace(Url.Link("EliminarAutor", new { id = autor.Id }), rel: "delete-author", metodo: "DELETE")); ;
        }

        // POST: api/Autores
        [HttpPost(Name = "CrearAutor")]
        public async Task<ActionResult> CrearAutor([FromBody] AutorCreacionDTO autorCreacion)
        {
            var autor = mapper.Map<Autor>(autorCreacion);

            await context.Autores.AddAsync(autor);
            await context.SaveChangesAsync();

            var autorDto = mapper.Map<AutorDTO>(autor);

            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autorDto);
        }

        // PUT: api/Autores/5
        [HttpPut("{id}", Name = "ActualizarAutorPut")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}", Name = "ActualizarAutorPatch")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorDto = mapper.Map<AutorCreacionDTO>(autor);

            patchDocument.ApplyTo(autorDto, ModelState);

            mapper.Map(autorDto, autor);

            var isValid = TryValidateModel(autor);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Borra un elemento específico
        /// </summary>
        /// <param name="id">Id del elemento a borrar</param>   
        [HttpDelete("{id}", Name = "EliminarAutor")]
        public async Task<ActionResult> Delete(int id)
        {
            var autorId = await context.Autores.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);

            if (autorId == default(int))
            {
                return NotFound();
            }

            context.Autores.Remove(new Autor()
            {
                Id = autorId
            });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
