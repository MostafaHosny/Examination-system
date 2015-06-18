using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineExaminationSystem.Extensions;
using OES.Model.Users;
using OES.Model.Examination;
using OES.Data;
using OES.Modules.Instructor;

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorBaseController : Controller
    {
        private Instructor _Instructor;
        public Instructor Instructor
        {
            get
            {
                _Instructor = _Instructor ?? (User.GetDbUser() as Instructor);
                return _Instructor;
            }
        }

        public List<Registration> Registrations
        {
            get
            {
                OESData db = new OESData();
                var registerations = db.Registrations.Include(r => r.Semester)
                    .Include(r => r.Course)
                    .Include(r => r.Chapters)
                    .Include(r => r.Chapters.Select(c => c.Questions))
                    .Where(r => r.InstructorId.Equals(Instructor.UserId, StringComparison.OrdinalIgnoreCase)).ToList();
                db.Dispose();
                return registerations;
            }
        }


        private ExamModule _ExamModule;
        public ExamModule ExamModule
        {
            get
            {
                _ExamModule = _ExamModule ?? new ExamModule();
                return _ExamModule;
            }
        }

    }
}