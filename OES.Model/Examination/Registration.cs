using OES.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Registration : BaseEntity
    {
        public Registration()
        {
            RegistrationId = GenerateKey();
        }
        [Key]
        public string RegistrationId { get; set; }

        [Display(Name = "Course")]
        [ForeignKey("CourseId")]
        
        public Course Course { get; set; }

        [Display(Name = "Instructor")]
        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }


        [Display(Name = "Semester")]
        [ForeignKey("SemesterId")]
        public Semester Semester { get; set; }


        [Required]
        public string SemesterId { get; set; }

        [Required]
        public string InstructorId { get; set; }

        [Required]
        public string CourseId { get; set; }

        public List<StudentRegistration> Students { get; set; }

        public List<Chapter> Chapters { get; set; }
        public List<Exam> Exams { get; set; }

        public List<ExamVersion> ExamVersions { get; set; }
    }
}
