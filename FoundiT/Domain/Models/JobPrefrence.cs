using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class JobPrefrence
    {
        [Key]
        public int ID { get; set; }

        public string RegisterID { get; set; }
        [ForeignKey("RegisterID")]

        public Register  register { get; set; } 

       public string KeySkills { get; set; }
        public string Industry { get; set; }
        public string Department { get; set; }
        public string PrefrredRole { get; set; }

        public string PrefrredLocation { get; set; }


    }
}
