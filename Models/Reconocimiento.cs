using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("REC_GENERAL")]
    public class Reconocimiento
    {
        [Column("device_id")]
        public String DevId {get; set;}

        [Column("ciudad_registro")]
        public String Ciudad {get; set;}

        [Column("sitio_registro")]
        public String Sitio {get; set;}

        [Column("time")]
        public DateTime DateTime {get; set;}

        [Column("reconocimientosDia")]
        public int Recos {get; set;}

        [Column("Personas")]
        public int Personas {get; set;}

        [Column("Alertas")]
        public int Alertas {get; set;}

        
    }
}