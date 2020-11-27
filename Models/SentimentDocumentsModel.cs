using System;
using System.Collections.Generic;

namespace prodamjuntocomcidadao_web.Models
{
    public partial class SentimentDocumentsModel
    {
        public List<Documents> documents { get; set; }
        public List<Errors> errors { get; set; }
    }  
    public partial class Documents
    {
        public string id { get; set; }
        public double score { get; set; }
        public string text { get; set; }
        public string language { get; set; }
    } 
    public partial class Errors
    {
    }
}
