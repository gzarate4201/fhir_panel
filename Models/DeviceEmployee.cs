
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    [Table("devices_employees")]
    public class DeviceEmployee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("device_id")]
        public string DevId { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }
        
        [Column("status")]
        public Boolean Status { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

    }
}

