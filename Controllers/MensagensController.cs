using Microsoft.AspNetCore.Mvc;
using prodamjuntocomcidadao_web.db;
using System.Collections.Generic;
using System.Linq;

namespace prodamjuntocomcidadao_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MensagensController : ControllerBase
    {
        private prodamjuntocomcidadaoContext _db { get; set; }
        public MensagensController(prodamjuntocomcidadaoContext contexto)
        {
            _db = contexto;
        }

        [HttpGet]
        public List<Mensagem> Get()
        {
            var todasAsMensagens = _db.Mensagem
                .OrderByDescending(msg => msg.Curtidas)
                .ToList<Mensagem>();
            return todasAsMensagens;
        }
    }
}