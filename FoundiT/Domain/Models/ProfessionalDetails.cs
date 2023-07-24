using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProfessionalDetails
        
    {
        [Key]
        public int ID { get; set; }
        public string RegiserID { get; set; }
        [ForeignKey("RegiserID")]
        public Register register { get; set; }
        public Experience Experience { get; set; }
        public string? CurrentLocation { get; set; }
    }
}
