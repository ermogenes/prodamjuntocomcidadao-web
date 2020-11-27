using Microsoft.AspNetCore.Mvc;
using prodamjuntocomcidadao_web.db;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace prodamjuntocomcidadao_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsApi")]
    public class TemasController : ControllerBase
    {
        private prodamjuntocomcidadaoContext _db { get; set; }
        public TemasController(prodamjuntocomcidadaoContext contexto)
        {
            _db = contexto;
        }

        [HttpGet]
        public List<Tema> Get()
        {
            var todosOsTemas = _db.Tema
                .OrderByDescending(tema => tema.Curtidas)
                .ToList<Tema>();
            return todosOsTemas;
        }

        [HttpPatch("{id}/curtir")]
        public ActionResult<dynamic> CurteTema(string id) {
            var tema = _db.Tema
                .SingleOrDefault(tema => tema.Id == id);
            
            if (tema == null)
            {
                return BadRequest();
            }

            tema.Curtidas += 1;
            _db.SaveChanges();

            return Ok(new { Curtidas = tema.Curtidas });
        }

    }
}