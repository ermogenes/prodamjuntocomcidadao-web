using Microsoft.AspNetCore.Mvc;
using prodamjuntocomcidadao_web.db;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using prodamjuntocomcidadao_web.Models;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace prodamjuntocomcidadao_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsApi")]
    public class MensagensController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private prodamjuntocomcidadaoContext _db { get; set; }
        public MensagensController(prodamjuntocomcidadaoContext contexto, IConfiguration configuration)
        {
            _db = contexto;
            Configuration = configuration;
        }

        [HttpGet]
        public ActionResult<List<Mensagem>> ObtemMensagens(int skip = 0)
        {
            var todasAsMensagens = _db.Mensagem
                .Include(msg => msg.Local)
                .Include(msg => msg.Tema)
                .Include(msg => msg.Tipo)
                .OrderByDescending(msg => msg.Curtidas)
                .Skip(skip)
                .Take(25)
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
        public async Task<ActionResult<Mensagem>> IncluiMensagem(Mensagem mensagem)
        {            
            string endpoint = "https://prodamjuntocomcidadao-cog-services.cognitiveservices.azure.com/text/analytics/v2.0/sentiment";

            using (var httpClient = new HttpClient())
            {
                mensagem.Id = System.Guid.NewGuid().ToString();

                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Configuration["CogApiKey"]);

                var jsonContent = JsonConvert.SerializeObject(new SentimentDocumentsModel {
                        documents = new List<Documents> {
                            new Documents {
                            id = mensagem.Id,
                            text = mensagem.Texto,
                            language = "pt",
                            }
                        }
                    }
                );

                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 

                using (var response = await httpClient.PostAsync(endpoint, contentString))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // System.Console.WriteLine(apiResponse);
                    var result = JsonConvert.DeserializeObject<SentimentDocumentsModel>(apiResponse);
                    mensagem.SentimentScore = result.documents[0].score;
                }
            }

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