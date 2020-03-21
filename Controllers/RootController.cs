using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoLibros.Models;

namespace ProyectoLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public ActionResult<IEnumerable<Enlace>> Get()
        {
            List<Enlace> enlaces = new List<Enlace>();

            // Aquí colocamos los links
            enlaces.Add(new Enlace(href: Url.Link("GetRoot", new { }), rel: "Self", metodo: "GET"));
            enlaces.Add(new Enlace(href: Url.Link("ObtenerAutores", new { }), rel: "autores", metodo: "GET"));
            enlaces.Add(new Enlace(href: Url.Link("CrearAutor", new { }), rel: "crear-autor", metodo: "POST"));
            enlaces.Add(new Enlace(href: Url.Link("ObtenerLibros", new { }), rel: "valores", metodo: "GET"));
            enlaces.Add(new Enlace(href: Url.Link("CrearLibro", new { }), rel: "crear-valor", metodo: "POST"));
            enlaces.Add(new Enlace(href: Url.Link("crear", new { }), rel: "crear-user", metodo: "POST"));
            enlaces.Add(new Enlace(href: Url.Link("login", new { }), rel: "login-user", metodo: "POST"));

            return enlaces;
        }
    }
}
