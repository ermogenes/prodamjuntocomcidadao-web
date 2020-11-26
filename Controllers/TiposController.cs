using Microsoft.AspNetCore.Mvc;
using prodamjuntocomcidadao_web.db;
using System.Collections.Generic;
using System.Linq;

namespace prodamjuntocomcidadao_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}