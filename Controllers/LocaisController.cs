using Microsoft.AspNetCore.Mvc;
using prodamjuntocomcidadao_web.db;
using System.Collections.Generic;
using System.Linq;

namespace prodamjuntocomcidadao_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocaisController : ControllerBase
    {
        private prodamjuntocomcidadaoContext _db { get; set; }
        public LocaisController(prodamjuntocomcidadaoContext contexto)
        {
            _db = contexto;
        }

        [HttpGet]
        public List<Local> Get()
        {
            var todosOsLocais = _db.Local
                .OrderByDescending(local => local.Curtidas)
                .ToList<Local>();
            return todosOsLocais;
        }

        [HttpPatch("{id}/curtir")]
        public ActionResult<dynamic> CurteLocal(string id) {
            var local = _db.Local
                .SingleOrDefault(local => local.Id == id);
            
            if (local == null)
            {
                return BadRequest();
            }

            local.Curtidas += 1;
            _db.SaveChanges();

            return Ok(new { Curtidas = local.Curtidas });
        }
    }
}