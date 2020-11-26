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
    }
}