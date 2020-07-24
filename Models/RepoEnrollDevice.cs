using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("REPO_ENROLL_DEVICE")]
    public class RepoEnrollDevice
    {

        [Column("user_id")]
        public String DocId {get; set;}

        [Column("device_id")]
        public String DevId {get; set;}

        [Column("Nombre")]
        public String Name {get; set;}

        [Column("ciudad_registro")]
        public String Ciudad {get; set;}

        [Column("sitio_registro")]
        public String Puerta {get; set;}

        [Column("intentos")]
        public int Intentos { get; set; }

        [Column("exitos")]
        public int Exitos { get; set; }

        [Column("Problemas")]
        public int Problemas { get; set; }


        [Column("hasPhoto")]
        public bool hasPhoto { get; set; }

        
    }
}