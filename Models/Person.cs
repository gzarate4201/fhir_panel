using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("person")]
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("msgType")]
        public string MsgType { get; set; }

        [Column("similar")]
        public int Similar { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("registerTime")]
        public DateTime RegisterTime { get; set; }

        [Column("temperature")]
        public float Temperature { get; set; }

        [Column("matched")]
        public int Matched { get; set; }

        [Column("imageUrl")]
        public string imageUrl { get; set; }
        
    }
}
