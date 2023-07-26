using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public  class EducationDetails
    {
        [Key]
        public int ID { get; set; }
        public string RegisterID { get; set; }
        [ForeignKey("RegisterID")]
        public Register register { get; set; }
        public string HighestQualification { get; set; }
        public string SelectYourField { get; set; }
        public string University_Insitute { get; set; }
        public string YearOfGraduation { get; set; }
        public Education EducationType { get; set; }
    }
}
