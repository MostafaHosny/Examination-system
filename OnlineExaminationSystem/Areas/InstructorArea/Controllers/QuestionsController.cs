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

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class QuestionsController : InstructorBaseController
    {
        public QuestionsController()
        {
            ViewBag.Page = "questions";
        }
        private OESData db = new OESData();

        // GET: InstructorArea/Questions
        public ActionResult Index(string id)
        {
            ExamModule module = new ExamModule();
            var chapter = module.GetChapterById(id);
            QuestionViewModel model = new QuestionViewModel()
            {
                Registrations = Registrations,
                SelectedChapter = chapter,
                SelectedRegistration = (chapter != null) ? chapter.Registration : null
            };
            return View(model);
        }


        // GET: InstructorArea/Questions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: InstructorArea/Questions/Create
        public ActionResult Create(string id)
        {
            return View(new Question() { ChapterId = id });
        }

        // POST: InstructorArea/Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionText,ChapterId,Type,Difficulty")] Question question)
        {
            if (ModelState.IsValid)
            {
                var result = ExamModule.AddQuestion(question);
                if (!result.Success)
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                    return View(result.ReturnObject);
                }
                return RedirectToAction("CreateAnswer", new { QuestionID = question.QuestionId });
                //return RedirectToAction("Index", new { id = question.ChapterId});
            }

            return View(question);
        }

        public ActionResult CreateAnswer(string QuestionID)
        {
            OESData module = new OESData();
            // var chapter = module.GetChapterById(id);

            Question CopyOfCreatedQuestion = module.Questions.Where(x => x.QuestionId == QuestionID).FirstOrDefault();
            CreatedQuestionViewModel model = new CreatedQuestionViewModel()
            {

                CreatedQuestion = CopyOfCreatedQuestion,
                HasAnswers = module.Answers.Where(x => x.QuestionId == QuestionID).ToList().Count() > 0,
                QuestionAnswers = module.Answers.Where(x => x.QuestionId == QuestionID).ToList(),
                ChapterAnswers = module.Answers.Where(x => x.Question.ChapterId == CopyOfCreatedQuestion.ChapterId).ToList()
            };
          //  model.ChapterAnswers.Except(model.QuestionAnswers.ToList());

            //for (int i = 0; i < model.ChapterAnswers.Count; i++)
            //{
            //    for (int I = 0; I < model.ChapterAnswers.Count - 1; I++)
            //    {
            //        if (model.ChapterAnswers[I].AnswerText == model.ChapterAnswers[i].AnswerText && model.ChapterAnswers[I].IsCorrectAnswer == model.ChapterAnswers[i].IsCorrectAnswer)
            //        {
            //            model.ChapterAnswers.RemoveAt(i);
            //           i--;
                     
            //        }
            //    }

            //}
                for (int I = 0; I < model.ChapterAnswers.Count; I++)
                {
                    for (int i = 0; i < model.ChapterAnswers.Count - 1; i++)
                    {
                        if (model.ChapterAnswers[I].AnswerText == model.ChapterAnswers[i].AnswerText && model.ChapterAnswers[I].IsCorrectAnswer == model.ChapterAnswers[i].IsCorrectAnswer && i != I)
                        {
                            model.ChapterAnswers.RemoveAt(I);
                            I--;
                            break;
                        }
                    }
                }

                for (int i = 0; i < model.QuestionAnswers.Count; i++)
                {
                    for (int I = 0; I < model.ChapterAnswers.Count; I++)
                    {
                        if (model.ChapterAnswers[I].AnswerText == model.QuestionAnswers[i].AnswerText && model.ChapterAnswers[I].IsCorrectAnswer == model.QuestionAnswers[i].IsCorrectAnswer)
                        {
                            model.ChapterAnswers.RemoveAt(I);
                            I--;
                        }
                    }
                    for (int I = 0 ; I < model.QuestionAnswers.Count - 1; I++)
                    {
                        if (model.QuestionAnswers[i].IsCorrectAnswer == model.QuestionAnswers[I].IsCorrectAnswer && model.QuestionAnswers[i].AnswerText == model.QuestionAnswers[I].AnswerText && i != I)
                        {
                            model.QuestionAnswers.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
            //model.ChapterAnswers
            return View(model);
        }
        [ActionName("CreateAnswerbyAnswerid")]
        public ActionResult CreateAnswer(string Answerid, string Questionid)
        {
            //if (ModelState.IsValid)
            //{
            var answer = db.Answers.Where(a => a.AnswerId == Answerid).FirstOrDefault();

            var NewAnswer = new Answer();
            NewAnswer.AnswerText = answer.AnswerText;
            NewAnswer.IsCorrectAnswer = answer.IsCorrectAnswer;
            NewAnswer.QuestionId = Questionid;
            //answer.NewQuestionAnswer.Question = answer.CreatedQuestion;
            var result = ExamModule.AddAnswer(NewAnswer);
            //if (db.Questions.Where(q => q.QuestionId == answer.CreatedQuestion.QuestionId).FirstOrDefault().Answers == null)
            //    db.Questions.Where(q => q.QuestionId == answer.CreatedQuestion.QuestionId).FirstOrDefault().Answers = new List<Answer>();
            //db.Questions.Where(q => q.QuestionId == answer.CreatedQuestion.QuestionId).FirstOrDefault().Answers.Add(answer.NewQuestionAnswer);
            //db.SaveChanges();


            if (result.Success)
            {

                return RedirectToAction("CreateAnswer", new { QuestionID = NewAnswer.QuestionId });

            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Key, err.Message);
                }
            }
            return RedirectToAction("CreateAnswer", new { id = NewAnswer.QuestionId });
            //}
            return View(answer);
        }

        public ActionResult RemoveAnswerbyAnswerid(string Answerid, string Questionid)
        {
            //if (ModelState.IsValid)
            //{

            Answer answer = db.Answers.Where (a=> a.AnswerId == Answerid && a.QuestionId == Questionid).FirstOrDefault();
            db.Answers.Remove(answer);
            db.SaveChanges();


            return RedirectToAction("CreateAnswer", new { QuestionID = Questionid });

        }
        public ActionResult QuestionAnswerIndex(string QuestionID)
        {
            OESData module = new OESData();
            // var chapter = module.GetChapterById(id);
            CreatedQuestionViewModel model = new CreatedQuestionViewModel()
            {
                CreatedQuestion = module.Questions.Where(x => x.QuestionId == QuestionID).FirstOrDefault(),
                HasAnswers = module.Answers.Where(x => x.QuestionId == QuestionID).ToList().Count() > 0,
                QuestionAnswers = module.Answers.Where(x => x.QuestionId == QuestionID).ToList()
            };

            return View(model);
        }
        // POST: InstructorArea/Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //[Bind(Include = "NewQuestionAnswer.AnswerText,NewQuestionAnswer.IsCorrectAnswer,NewQuestionAnswer.QuestionId")]
        public ActionResult CreateAnswer(CreatedQuestionViewModel answer)
        {
            //if (ModelState.IsValid)
            //{

            //answer.NewQuestionAnswer.Question = answer.CreatedQuestion;
            answer.NewQuestionAnswer.QuestionId = answer.CreatedQuestion.QuestionId;

            var result = ExamModule.AddAnswer(answer.NewQuestionAnswer);
            //if (db.Questions.Where(q => q.QuestionId == answer.CreatedQuestion.QuestionId).FirstOrDefault().Answers == null)
            //    db.Questions.Where(q => q.QuestionId == answer.CreatedQuestion.QuestionId).FirstOrDefault().Answers = new List<Answer>();
            //db.Questions.Where(q => q.QuestionId == answer.CreatedQuestion.QuestionId).FirstOrDefault().Answers.Add(answer.NewQuestionAnswer);
            //db.SaveChanges();


            if (result.Success)
            {

                return RedirectToAction("CreateAnswer", new { QuestionID = answer.CreatedQuestion.QuestionId });

            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Key, err.Message);
                }
            }
            return RedirectToAction("CreateAnswer", new { id = answer.NewQuestionAnswer.QuestionId });
            //}
            return View(answer);
        }

        // GET: InstructorArea/Questions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: InstructorArea/Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionId,QuestionText,ChapterId,Type,Difficulty")] Question question)
        {
            if (ModelState.IsValid)
            {
                var result = ExamModule.UpdateQuestion(question);
                if (result.Success)
                {
                    return RedirectToAction("Index", new { id = result.ReturnObject.ChapterId });
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                }
            }
            return View(question);

        }

        // GET: InstructorArea/Questions/Delete/5
        public ActionResult Delete(string id)
        {
            var question = db.Questions.Include(e => e.Answers).Where(e =>e.QuestionId ==id).FirstOrDefault();
            
            string chapterId = question.ChapterId;

            for (int i = 0; i < question.Answers.Count; i++)
            {
                db.Answers.Remove(question.Answers[i]);
 
            }
               
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = chapterId });
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
