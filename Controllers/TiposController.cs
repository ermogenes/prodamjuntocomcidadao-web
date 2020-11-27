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
    public class TiposController : ControllerBase
    {
        private prodamjuntocomcidadaoContext _db { get; set; }
        public TiposController(prodamjuntocomcidadaoContext contexto)
        {
            _db = contexto;
        }

        [HttpGet]
        public List<Tipo> Get()
        {
            var todosOsTipos = _db.Tipo
                .OrderByDescending(tipo => tipo.Curtidas)
                .ToList<Tipo>();
            return todosOsTipos;
        }

        [HttpPatch("{id}/curtir")]
        public ActionResult<dynamic> CurteTipo(string id) {
            var tipo = _db.Tipo
                .SingleOrDefault(tipo => tipo.Id == id);
            
            if (tipo == null)
            {
                return BadRequest();
            }

            tipo.Curtidas += 1;
            _db.SaveChanges();

            return Ok(new { Curtidas = tipo.Curtidas });
        }
    }
}