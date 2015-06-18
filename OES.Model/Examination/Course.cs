using OES.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Course : BaseEntity
    {
        public Course()
        {
            CourseId = GenerateKey();
        }
        [Key]
        public string CourseId { get; set; }

      
        public string Code { get; set; }
        [Required]
        public string Title { get; set; }


        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        

        public List<Registration> Registrations { get; set; }
    }

    public enum coursecode
    {
        CS,
        IS
    }
}
