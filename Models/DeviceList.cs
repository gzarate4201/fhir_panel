using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    // Definicion del Objeto Devices (tabla para almacenar la informacion de configuracion de los dispositivos)
    [Table("devices")]
    public class DeviceList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("device_id")]
        public string DevId { get; set; }
        
    }
}
