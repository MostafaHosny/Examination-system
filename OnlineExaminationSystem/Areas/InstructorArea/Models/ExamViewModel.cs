using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OES.Data;
using OES.Model.Examination;
//using OnlineExaminationSystem.Areas.InstructorArea.Models;
//using OES.Modules.Instructor;
//using OES.Modules.Common;
//using OnlineExaminationSystem.Models;
//using System.ComponentModel.DataAnnotations.Schema;
using OES.Model.Users;

namespace OnlineExaminationSystem.Areas.InstructorArea.Models
{
    public class ExamViewModel
    {
        public Registration SelectedRegistration { get; set; }

        public List<Registration> Registrations { get; set; }

        public string SelectedExamId { get; set; }
    }

    public class StudentExamViewModel
    {
        public Registration SelectedRegistration { get; set; }
        public List<Student> StudentList { get; set; }
        public List<Registration> Registrations { get; set; }

        public string SelectedExamId { get; set; }
    }
}