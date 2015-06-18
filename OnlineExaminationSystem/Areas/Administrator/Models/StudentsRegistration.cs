using OES.Model.Examination;
using OES.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Administrator.Models
{
    public class StudentsRegistration
    {

        public List<Student> Students { get; set; }

        public Registration Registration { get; set; }
    }
}