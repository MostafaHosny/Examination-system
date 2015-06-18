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

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class AnswersController : InstructorBaseController
    {
        private OESData db = new OESData();

        public AnswersController()
        {
            ViewBag.Page = "questions";
        }

        // GET: InstructorArea/Answers
        public ActionResult Index(string id)
        {
            AnswerViewModel model = new AnswerViewModel();
            model.SelectedQuestion = ExamModule.GetQuestion(id);
            return View(model);
        }


        // GET: InstructorArea/Answers/Create
        public ActionResult Create(string id)
        {
            return View(new Answer() { QuestionId = id });
        }

        // POST: InstructorArea/Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnswerText,IsCorrectAnswer,QuestionId")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                var result = ExamModule.AddAnswer(answer);
                if (result.Success)
                {
                    return RedirectToAction("Index", new { id = result.ReturnObject.QuestionId });
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                }
                return RedirectToAction("Index", new { id = answer.QuestionId });
            }
            return View(answer);
        }

        // GET: InstructorArea/Answers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: InstructorArea/Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnswerId,AnswerText,IsCorrectAnswer,QuestionId")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                var result = ExamModule.UpdateAnswer(answer);
                if (result.Success)
                {
                    return RedirectToAction("CreateAnswer", "Questions", new { QuestionID = result.ReturnObject.QuestionId });
                   // return RedirectToAction("Index", "" , new { id = result.ReturnObject.QuestionId });
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                }
            }
            return View(answer);
        }


        public ActionResult Delete(string id)
        {
            Answer answer = db.Answers.Find(id);
            string questionId = answer.QuestionId;
            db.Answers.Remove(answer);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = questionId });
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
