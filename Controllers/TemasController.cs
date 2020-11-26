using Microsoft.AspNetCore.Mvc;
using prodamjuntocomcidadao_web.db;
using System.Collections.Generic;
using System.Linq;

namespace prodamjuntocomcidadao_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}