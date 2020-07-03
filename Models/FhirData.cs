using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("fhir_data")]
    public class FhirData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("tipo_documento")]
        public string TipoDoc { get; set; }

        [Column("numero_documento")]
        public Int64 NumDoc { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [Column("temperatura")]
        public double Temperature { get; set; }

        [Column("ciudad_registro")]
        public string CiudadReg { get; set; }

        [Column("sitio_registro")]
        public string SitioReg { get; set; }

        [Column("sitio_registro_id")]
        public string SitioRegId { get; set; }

        [Column("latitud")]
        public double Lat { get; set; }

        [Column("longitud")]
        public double Lon { get; set; }

        [Column("nit")]
        public string Nit { get; set; }

        [Column("reportado")]
        public int Report { get; set; }

        [Column("instrumento")]
        public string Instrumento { get; set; }

        [Column("tipo_calibracion")]
        public string TipoCal { get; set; }

        [Column("tipo_medicion")]
        public string TipoMed { get; set; }

        [Column("valor_calibracion")]
        public double ValCal { get; set; }

        [Column("id_lenel")]
        public Int64 IdLenel { get; set; }
        
    }
}
