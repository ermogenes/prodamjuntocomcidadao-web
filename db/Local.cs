using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace prodamjuntocomcidadao_web.db
{
    public partial class Local
    {
        public Local()
        {
            Mensagem = new HashSet<Mensagem>();
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public int Curtidas { get; set; }

        [JsonIgnore]
        public virtual ICollection<Mensagem> Mensagem { get; set; }
    }
}
