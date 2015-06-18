using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.StudentArea.Models
{
    public class ExamViewModel
    {
        public Registration SelectedRegistration { get; set; }

        public List<Registration> Registrations { get; set; }

        public string SelectedExamId { get; set; }
    }

    public class ExamResultViewModel
    {
        public Registration SelectedRegistration { get; set; }

        public List<Registration> Registrations { get; set; }

        public string SelectedExamId { get; set; }
    }
}