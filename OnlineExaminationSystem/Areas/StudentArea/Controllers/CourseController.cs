using OES.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineExaminationSystem.Extensions;

namespace OnlineExaminationSystem.Areas.StudentArea.Controllers
{
    public class CourseController : StudentBaseController
    {
        private OESData db = new OESData();

        public CourseController()
        {
            ViewBag.Page = "course";
        }
        // GET: InstructorArea/Course
        public ActionResult Index()
        {
            return View(Registrations);
        }
    }
}