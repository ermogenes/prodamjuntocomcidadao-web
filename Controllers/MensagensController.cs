using Microsoft.AspNetCore.Mvc;
using prodamjuntocomcidadao_web.db;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult<List<Mensagem>> ObtemMensagens()
        {
            var todasAsMensagens = _db.Mensagem
                .Include(msg => msg.Local)
                .Include(msg => msg.Tema)
                .Include(msg => msg.Tipo)
                .OrderByDescending(msg => msg.Curtidas)
                .ToList<Mensagem>();
            return Ok(todasAsMensagens);
        }

        [HttpPatch("{id}/curtir")]
        public ActionResult<dynamic> CurteMensagem(string id) {
            var mensagem = _db.Mensagem
                .SingleOrDefault(msg => msg.Id == id);
            
            if (mensagem == null)
            {
                return BadRequest();
            }

            mensagem.Curtidas += 1;
            _db.SaveChanges();

            return Ok(new { Curtidas = mensagem.Curtidas });
        }

        [HttpPost]
        public ActionResult<Mensagem> IncluiMensagem(Mensagem mensagem)
        {
            mensagem.Id = System.Guid.NewGuid().ToString();
            mensagem.Curtidas = 0;
            mensagem.Data = System.DateTime.Now.ToString();

            if (string.IsNullOrEmpty(mensagem.LocalId))
            {
                mensagem.LocalId = null;
            }

            if (string.IsNullOrEmpty(mensagem.TemaId))
            {
                mensagem.TemaId = null;
            }

            if (string.IsNullOrEmpty(mensagem.TipoId))
            {
                mensagem.TipoId = null;
            }

            _db.Add(mensagem);
            _db.SaveChanges();
            return CreatedAtAction(nameof(ObtemMensagens), null, mensagem);
        }
    }
}