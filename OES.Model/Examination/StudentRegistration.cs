using OES.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class StudentRegistration : BaseEntity
    {
        public StudentRegistration()
        {
            StudentRegistrationId = GenerateKey();
        }
        public string StudentRegistrationId { get; set; }

        public string UserId { get; set; }

        public string RegistrationId { get; set; }

        [ForeignKey("UserId")]
        public Student Student { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration Registration { get; set; } 
    }
}
