using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("DISTRIBUCION EVENTOS")]
    public class RecoDia
    {
        [Column("Fecha")]
        public String Fecha {get; set;}

        [Column("device_id")]
        public String DevId {get; set;}

        [Column("regional")]
        public String Regional {get; set;}

        [Column("ciudad_registro")]
        public String Ciudad {get; set;}

        [Column("sitio_registro")]
        public String Sitio {get; set;}

        [Column("pta_lenel")]
        public String Puerta {get; set;}

        [Column("empresa")]
        public String Empresa {get; set;}

        [Column("reconocimientosDia")]
        public int Recos {get; set;}

        [Column("Personas")]
        public int Personas {get; set;}

        [Column("Alertas")]
        public int Alertas {get; set;}

        
    }
}