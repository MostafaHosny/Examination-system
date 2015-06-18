using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OES.Data;
using OES.Model.Examination;
using OnlineExaminationSystem.Areas.StudentArea.Models;
using OES.Modules.Instructor;
using OES.Modules.Common;
using OnlineExaminationSystem.Models;
using OES.Model.Users;
using System.Web;


namespace OnlineExaminationSystem.Areas.StudentArea.Controllers
{
    public class ExamsController : StudentBaseController
    {
        private OESData db = new OESData();

        public ExamsController()
        {
            ViewBag.Page = "exams";
        }

        // GET: InstructorArea/Exams
        public ExamViewModel GetExamViewModel()
        {

            var stdname = HttpContext.User.Identity.Name;
            var student = (from std in db.Students where std.UserName == stdname select std).First();

            List<string> CoursesID = (from course in db.Courses
                                      join reg in db.Registrations on course.CourseId equals reg.CourseId
                                      join studentreg in db.StudentRegistration on reg.RegistrationId equals studentreg.RegistrationId
                                      where studentreg.UserId == student.UserId
                                      select course.CourseId).ToList();
            var model = new ExamViewModel();
            //if (!string.IsNullOrWhiteSpace(id))
            //{
            //    model.SelectedRegistration = ExamModule.GetRegistrationForValidExams(id);
            //}
            //else
            //{
            model.Registrations = new List<Registration>();
            foreach (var MyCourseID in CoursesID)
                model.Registrations.Add(ExamModule.GetRegistrationForAllExams(MyCourseID));
            return model;

        }

        public ExamResultViewModel GetExamResultsViewModel()
        {

            var stdname = HttpContext.User.Identity.Name;
            var student = (from std in db.Students where std.UserName == stdname select std).First();

            List<string> CoursesID = (from course in db.Courses
                                      join reg in db.Registrations on course.CourseId equals reg.CourseId
                                      join studentreg in db.StudentRegistration on reg.RegistrationId equals studentreg.RegistrationId
                                      where studentreg.UserId == student.UserId
                                      select course.CourseId).ToList();
            var model = new ExamResultViewModel();
            //if (!string.IsNullOrWhiteSpace(id))
            //{
            //    model.SelectedRegistration = ExamModule.GetRegistrationForValidExams(id);
            //}
            //else
            //{
            model.Registrations = new List<Registration>();
            foreach (var MyCourseID in CoursesID)
                model.Registrations.Add(ExamModule.GetRegistrationForAllExamsResults(MyCourseID));
            return model;
        }
        public ActionResult Index(string id = null)
        {

            
            //}
            //model.Registrations = Registrations;
            return View(GetExamViewModel());
        }

        public ActionResult Select(string id)
        {
            return RedirectToAction("Index", new { id = id });
        }

        // GET: InstructorArea/Exams/Create
        public ActionResult Create(string id)
        {

            return View(new Exam() { RegistrationId = id });
        }

        // POST: InstructorArea/Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      

       

        public ActionResult ExamResults()
        {
            var stdname = HttpContext.User.Identity.Name;
            var student = (from std in db.Students where std.UserName == stdname select std).First(); //db.Students.Where(s => s.UserName == stdname).FirstOrDefault();
            //ExamViewModel CurrentExamViewModel = GetExamViewModel();
            ExamResultViewModel CurrentExamResultViewModel = GetExamResultsViewModel();
            foreach (var reg in CurrentExamResultViewModel.Registrations)
            {
                foreach (var ExamVer in reg.ExamVersions)
                {
                    if (ExamVer.StudentId == student.UserId)
                    ExamVer.StudentScore = (float)ExamVersionViewModel.BuildViewModel(ExamVer).UserScore;
                }
            }

            return View();
        }

        public ActionResult Generate(string id)
        {


            var stdname = HttpContext.User.Identity.Name;
            var student = (from std in db.Students where std.UserName == stdname select std).First(); //db.Students.Where(s => s.UserName == stdname).FirstOrDefault();
            ExamViewModel CurrentExamViewModel = GetExamViewModel();

             
           
            var exam = db.Exams
                 .Include(e => e.Registration).Include(e=> e.ExamVersions)
                 .FirstOrDefault(e => e.ExamId.Equals(id, StringComparison.OrdinalIgnoreCase));


            bool check = true;
            foreach (var ExamVer in exam.ExamVersions)
            {
                if (ExamVer.StudentId == student.UserId ) 
                {
                    check = false ;
                }
            }

            if (check)
            {
                GenerateExamModule module = new GenerateExamModule();
                var result = module.GenerateExamVersionForStudent(id ,student);
                if (result.Success)
                {
                    // student.StudentExamVersions.Add(result.ReturnObject);
                    //result.ReturnObject.Student.StudentExamVersions.Add(student);
                    //  //student.StudentExamVersions.Add(result.ReturnObject);

                    return View("Exam", ExamVersionViewModel.BuildViewModel(result.ReturnObject));
                }
                else
                {
                    ModelState.AddModelError("1253", "Pending for instructor generation ");
                    //foreach (var err in result.Errors)
                    //{
                    //    ModelState.AddModelError(err.Key, err.Message);
                    //}
                    //var model = new ExamViewModel();
                   // model.SelectedRegistration = ExamModule.GetRegistrationForExams(result.ReturnObject.RegistrationId);
                    //model.Registrations = Registrations;
                    CurrentExamViewModel.SelectedExamId = id;
                    return View("Index", CurrentExamViewModel);
                }
            }
            else
            {
                ModelState.AddModelError("125633", "You submitted the exam before");

             //   var model = new ExamViewModel();
                //model.SelectedRegistration = ExamModule.GetRegistrationForExams(exam.RegistrationId);
              //  model.Registrations = Registrations;
                CurrentExamViewModel.SelectedExamId = id;
                return View("Index", CurrentExamViewModel);
            }
        }

        public ActionResult GenerateDemo(string id)
        {

            //Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AppendHeader("Cache-Control", "no-cache");

            GenerateExamModule module = new GenerateExamModule();
            var exam = db.Exams.Where( a=> a.ExamId == id).Include(a=>a.Registration.Course).FirstOrDefault();
           
            ;
            var result = module.GenerateExamVersion(id);

            if (result.Success)
            {
                ViewBag.ExamTitile = exam.Registration.Course.Title;
                return View("Exam", ExamVersionViewModel.BuildViewModel(result.ReturnObject));
            }
            else
            {
                //foreach (var err in result.Errors)
                //{
                //    ModelState.AddModelError(err.Key, err.Message);
                //}
                ModelState.AddModelError("12583", "Pending for instructor generation ");

                ExamViewModel CurrentExamViewModel = GetExamViewModel();
                CurrentExamViewModel.SelectedExamId = id;
                return View("Index", CurrentExamViewModel);
            }
        }

        public ActionResult Submit(FormCollection inputs)
        {
            string examVersionId = inputs["ExamVersionId"];
            var version = db.ExamVersions.Include(v => v.Questions)
                .Include(v => v.Questions.Select(q=> q.Answers))
                .FirstOrDefault(v => v.ExamVersionId.Equals(examVersionId, StringComparison.OrdinalIgnoreCase));
            if (version != null)
            {
                //var viewModel = ExamVersionViewModel.BuildViewModel(version);
                foreach(var question in version.Questions)
                {
                    if (inputs.AllKeys.Contains(question.QuestionVersionId))
                    {
                        var selectedAnswerId = inputs[question.QuestionVersionId];
                        var selectedAnswer = question.Answers.FirstOrDefault(a => a.AnswerVersionId.Equals(selectedAnswerId, StringComparison.OrdinalIgnoreCase));
                        if (selectedAnswer != null)
                        {
                            selectedAnswer.IsThisUserAnswer = true;
                        }
                    }
                }
                version.StudentScore = (float)ExamVersionViewModel.BuildViewModel(version).UserScore;
                version.StudentGrade = CalcluateGrade(version.TotalScore, version.StudentScore);
                db.SaveChanges();
            }
            return View("ExamResult", ExamVersionViewModel.BuildViewModel(version));
        }


        public string CalcluateGrade (float TotalScore , float StudentScore )
        {
            string grade ;
            float persentage = (StudentScore / TotalScore) * 100;
            if (persentage >= 85)
                grade = "A";
            else if (persentage <= 85 && persentage >= 75)
                grade = "B";
            else if (persentage <= 75 && persentage >= 65)
                grade = "C";
            else if (persentage <= 65 && persentage >= 50)
                grade = "D";
            else
                grade = "Fail";
           
            return grade;

        }
        public ActionResult SeeAnswers(string id)
        {
            
            var version = db.ExamVersions.Include(v => v.Questions)
                .Include(v => v.Questions.Select(q => q.Answers))
                .FirstOrDefault(v => v.ExamVersionId.Equals(id, StringComparison.OrdinalIgnoreCase));
         
            return View("ExamResult", ExamVersionViewModel.BuildViewModel(version));
        }

        public ActionResult StudentReport()
        {
            var stdname = HttpContext.User.Identity.Name;
            var student = (from std in db.Students where std.UserName == stdname select std).First(); //db.Students.Where(s => s.UserName == stdname).FirstOrDefault();
            var examVersions = db.ExamVersions
                  .Include(e => e.Exam).Include(e => e.Registration).Include(e => e.Registration.Semester).Include(e => e.Registration.Course).Include(a =>a.Student).Where(a => a.StudentId == student.UserId).ToList();

            return View("StudentReport", examVersions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
