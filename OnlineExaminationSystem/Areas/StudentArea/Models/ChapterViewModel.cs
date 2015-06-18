using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.StudentArea.Models
{
    public class ChapterViewModel
    {
        public List<Registration> Registrations { get; set; }

        public Registration SelectedRegisteration { get; set; }
    }
}