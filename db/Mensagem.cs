using System;
using System.Collections.Generic;

namespace prodamjuntocomcidadao_web.db
{
    public partial class Mensagem
    {
        public string Id { get; set; }
        public string Texto { get; set; }
        public int Curtidas { get; set; }
        public string TipoId { get; set; }
        public string LocalId { get; set; }
        public string TemaId { get; set; }
        public string Data { get; set; }

        public virtual Local Local { get; set; }
        public virtual Tema Tema { get; set; }
        public virtual Tipo Tipo { get; set; }
    }
}
