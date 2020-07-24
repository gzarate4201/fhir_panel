using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("SOPO_RECO_PERSONA_DIA")]
    public class SopoRecoPersona
    {
        [Column("user_id")]
        public Int32 UserId {get; set;}


        [Column("time")]
        public DateTime Time {get; set;}
        
        [Column("Fecha")]
        public string Fecha {get; set;}

        [Column("name")]
        public string Name {get; set;}

        [Column("empresa")]
        public string Empresa {get; set;}

        [Column("ciudad_registro")]
        public String Ciudad {get; set;}
        
    }
}
