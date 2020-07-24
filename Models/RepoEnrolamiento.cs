using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("REPO_ENROLAMIENTO")]
    public class RepoEnrolamiento
    {
        [Column("Fecha")]
        public DateTime Fecha {get; set;}

        [Column("time")]
        public DateTime Time {get; set;}

        [Column("documento")]
        public String DocId {get; set;}

        [Column("name")]
        public String Name {get; set;}

        [Column("empresa")]
        public String Empresa {get; set;}

        [Column("hasPhoto")]
        public bool hasPhoto { get; set; }

        
    }
}