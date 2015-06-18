using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Semester : BaseEntity
    {
        public Semester() {
            SemesterId = GenerateKey();
          
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

        }
        [Key]
        public string SemesterId { get; set; }

   
        public string SemesterTitle { get; set; }

        public List<Registration> Registrations { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        public SemesterName SemesterName { get; set; }
      [Required]
        public int SemesterYear { get; set; }
        
    }

    public enum SemesterName
    {
        Spring,
        Fall,
        Summer

    }
   
  
}
