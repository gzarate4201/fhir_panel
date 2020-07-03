using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    // Definicion del Objeto Devices (tabla para almacenar la informacion de configuracion de los dispositivos)
    [Table("device_site")]
    public class DeviceSite
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("device_id")]
        public string DevId { get; set; }

        [Column("latitud")]
        public double Lat { get; set; }

        [Column("longitud")]
        public double Lon { get; set; }

        [Column("ciudad_registro")]
        public string CiudadReg { get; set; }

        [Column("sitio_registro")]
        public string SitioReg { get; set; }

        [Column("sitio_registro_id")]
        public int SitioRegId { get; set; }

        [Column("nit")]
        public string Nit { get; set; }
        
    }
}
