using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("upload_person")]
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("msgType")]
        public int MsgType { get; set; }

        [Column("similar")]
        public double Similar { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("time")]
        public DateTime RegisterTime { get; set; }

        [Column("temperature")]
        public double Temperature { get; set; }

        [Column("mask")]
        public int Mask { get; set; }

        [Column("matched")]
        public int Matched { get; set; }

        [Column("device_id")]
        public string DevId { get; set; }

        [Column("imageUrl")]
        public string imageUrl { get; set; }
        
        //public List<Employee> Employee { get; set; }
    }
}
