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
using OnlineExaminationSystem.Areas.InstructorArea.Models;
using OES.Modules.Instructor;
using OES.Modules.Common;
using OnlineExaminationSystem.Models;

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class ExamsController : InstructorBaseController
    {
        private OESData db = new OESData();

        public ExamsController()
        {
            ViewBag.Page = "exams";
        }

        // GET: InstructorArea/Exams
        public ActionResult Index(string id = null)
        {
            var model = new ExamViewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model.SelectedRegistration = ExamModule.GetRegistrationForExams(id);
            }
           
            model.Registrations = Registrations;
            return View(model);
        }

        public ActionResult Select(string id)
        {
            return RedirectToAction("Index", new { id = id });
        }

        // GET: InstructorArea/Exams/Create
        public ActionResult Create(string id)
        {
            Exam myExam = new Exam();
            myExam.RegistrationId = id;
            //var allChapters = db.Chapters.Where(e => e.RegistrationId == id).ToList();
            myExam.ExamChapters = new List<ExamChapter>();
            //foreach (var Chap in allChapters)
            //{
            //    ExamChapter examchap = new ExamChapter();
            //    examchap.ChapterName = Chap.Title;
            //    examchap.ChapterId = Chap.ChapterId;
            //    myExam.ExamChapters.Add(examchap);
            //}
            return View(myExam);
            //return View(new Exam() { RegistrationId = id });
        }

      
        // POST: InstructorArea/Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                var result = ExamModule.AddExam(exam);
                if (result.Success)
                {
                    return RedirectToAction("Index", new { id = exam.RegistrationId });
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                }

            }

            return View(exam);
        }

        //[ActionName("CreateAnswerbyAnswerid")]
        public ActionResult AddChapter(string mychapterId,string myexamId)
        {
            var chapter = db.Chapters.Where(a => a.ChapterId == mychapterId).FirstOrDefault();
            var exam = db.Exams.Where(a => a.ExamId == myexamId).Include(a=>a.ExamChapters).FirstOrDefault();
          
            if (chapter != null && exam != null)
            {
               
               
                    ExamChapter examchapter = new ExamChapter()
                    {
                        ExamId = myexamId,
                        ChapterId = mychapterId,
                        ChapterName = chapter.Title
                    };
                   // exam.ExamChapters = exam.ExamChapters ?? new List<ExamChapter>();

                   var found =  exam.ExamChapters.Find(a => a.ChapterId == mychapterId);
                   if (found == null)
                   {
                       exam.ExamChapters.Add(examchapter);

                       db.SaveChanges();
                   }
                }
            
            var model = new ExamViewModel();
            model.SelectedRegistration = ExamModule.GetRegistrationForExams(exam.RegistrationId);
            model.Registrations = Registrations;
            model.SelectedExamId = exam.ExamId;
            return View("Index", model);
          
         
        }

        public ActionResult RemoveChapter(string mychapterId,string myexamId)
        {

            var exam = db.Exams.Where(e => e.ExamId == myexamId).Include(e => e.ExamChapters).FirstOrDefault();
            var examChap = db.ExamChapters.Where(e => e.ChapterId == mychapterId).FirstOrDefault();
            exam.ExamChapters.Remove(examChap);
            db.ExamChapters.Remove(examChap);
            db.SaveChanges();

            var model = new ExamViewModel();
            model.SelectedRegistration = ExamModule.GetRegistrationForExams(exam.RegistrationId);
            model.Registrations = Registrations;
            model.SelectedExamId = exam.ExamId;
            return View("Index", model);
           
            

        }

        public ActionResult Delete(string id)
        {
            
                Exam exam = db.Exams.Include(a=>a.ExamChapters).Where(a=>a.ExamId == id).FirstOrDefault();
          
                string regId = exam.RegistrationId;

                try
                {
                    for (int i = 0; i < exam.ExamChapters.Count; i++)
                    {
                        exam.ExamChapters.Remove(exam.ExamChapters[i]);
                       // db.ExamChapters.Remove(exam.ExamChapters[i]);
                    }
                    db.Exams.Remove(exam);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = regId });
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("15233", "some student took this exam before so you can't delete it");
                    var model = new ExamViewModel();
                    model.SelectedRegistration = ExamModule.GetRegistrationForExams(regId);
                    model.Registrations = Registrations;
                    model.SelectedExamId = id;
                    return View("Index", model);
                }
                
        }

        public ActionResult Generate(string id)
        {
            GenerateExamModule module = new GenerateExamModule();
            var result = module.GenerateExamVersionforInstructor(id);
            if (result.Success)
            {
                return View("Exam", ExamVersionViewModel.BuildViewModel(result.ReturnObject));
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Key, err.Message);
                }
                var model = new ExamViewModel();
                model.SelectedRegistration = ExamModule.GetRegistrationForExams(result.ReturnObject.RegistrationId);
                model.Registrations = Registrations;
                model.SelectedExamId = id;
                return View("Index", model);
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
                        var selectedAnswer =question.Answers.FirstOrDefault(a => a.AnswerVersionId.Equals(selectedAnswerId, StringComparison.OrdinalIgnoreCase));
                        if (selectedAnswer != null)
                        {
                            selectedAnswer.IsThisUserAnswer = true;
                        }
                    }
                }
                db.SaveChanges();
            }
            return View("ExamResult", ExamVersionViewModel.BuildViewModel(version));
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
