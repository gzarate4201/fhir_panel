using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web;

namespace AspStudio.Models
{
    public class EmailFormModel
    {
        [Required, Display(Name="Indra - Reports")]
        public string FromName { get; set; }
        [Required, Display(Name = "reportes@qaingenieros.com"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }

        [Required]
        public string Subject { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Attach { get; set; }


    }
}