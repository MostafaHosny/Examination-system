using OES.Data;
using OES.Model.Examination;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using OES.Modules.Core;
using OES.Modules.Common;

namespace OES.Modules.Instructor
{
    public class ExamModule
    {
        #region Chapter
        public List<Question> GetChapterQuestions(string chapterId)
        {
            OESData db = new OESData();
            return db.Questions.Include(q => q.Chapter).Where(q => q.ChapterId.Equals(chapterId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Chapter GetChapterById(string chapterId)
        {
            OESData db = new OESData();
            return db.Chapters.Include(c => c.Questions)
                .Include(c => c.Questions.Select(q => q.Answers))
                .Include(c => c.Questions.Select(q => q.Chapter))
                .Include(c => c.Registration)
                .Include(c => c.Registration.Semester)
                .Include(c => c.Registration.Course)
                .FirstOrDefault(c => c.ChapterId.Equals(chapterId, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Question
        public List<Answer> GetAnswers(string questionId)
        {
            OESData db = new OESData();
            return db.Answers.Where(a => a.QuestionId.Equals(questionId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Question GetQuestion(string questionId)
        {
            OESData db = new OESData();
            return db.Questions.Include(q => q.Answers)
                .Include(q => q.Chapter)
                .Include(q => q.Chapter.Registration)
                .FirstOrDefault(q => q.QuestionId.Equals(questionId, StringComparison.OrdinalIgnoreCase));
        }

        public Result<Question> AddQuestion(Question question)
        {
            var result = new Result<Question>();
            OESData db = new OESData();
            try
            {

                db.Questions.Add(question);
                if (question.Type.Equals(QuestionType.TrueFalse))
                {
                    question.Answers = question.Answers ?? new List<Answer>();
                    question.Answers.Add(new Answer()
                    {
                        QuestionId = question.QuestionId,
                        Question = question,
                        AnswerText = "True",
                        IsCorrectAnswer = true
                    });
                    question.Answers.Add(new Answer()
                    {
                        QuestionId = question.QuestionId,
                        Question = question,
                        AnswerText = "False"
                    });
                }
                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = question;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = question;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(){ Key="", Message = ex.Message}
                };
            }
            return result;
        }


        public Result<Answer> UpdateAnswer(Answer answer)
        {
            Result<Answer> result = new Result<Answer>();
            OESData db = new OESData();
            try
            {
                var dbAnswer = db.Answers.FirstOrDefault(a => a.AnswerId.Equals(answer.AnswerId, StringComparison.OrdinalIgnoreCase));
                dbAnswer.AnswerText = answer.AnswerText;
                dbAnswer.IsCorrectAnswer = answer.IsCorrectAnswer;
                if (answer.IsCorrectAnswer)
                {
                    foreach (var otherCorrectAnswer in db.Answers.Where(a => a.QuestionId.Equals(answer.QuestionId, StringComparison.OrdinalIgnoreCase) && a.IsCorrectAnswer))
                    {
                        otherCorrectAnswer.IsCorrectAnswer = false;
                    }
                }
                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = dbAnswer;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = answer;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(){ Key="", Message = ex.Message}
                };
            }
            return result;
        }


        public Result<Answer> AddAnswer(Answer answer)
        {
            Result<Answer> result = new Result<Answer>();
            OESData db = new OESData();
            try
            {
                db.Answers.Add(answer);
                if (answer.IsCorrectAnswer)
                {
                    foreach (var otherCorrectAnswer in db.Answers.Where(a => a.QuestionId.Equals(answer.QuestionId, StringComparison.OrdinalIgnoreCase) && a.IsCorrectAnswer))
                    {
                        otherCorrectAnswer.IsCorrectAnswer = false;
                    }
                }
                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = answer;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = answer;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(){ Key="", Message = ex.Message}
                };
            }
            return result;
        }
        public Result<Question> UpdateQuestion(Question question)
        {
            Result<Question> result = new Result<Question>();
            OESData db = new OESData();
            try
            {
                var dbQuestion = db.Questions.FirstOrDefault(a => a.QuestionId.Equals(question.QuestionId, StringComparison.OrdinalIgnoreCase));
                dbQuestion.QuestionText = question.QuestionText;
                dbQuestion.Difficulty = question.Difficulty;
                dbQuestion.Type = question.Type;

                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = dbQuestion;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = question;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(){ Key="", Message = ex.Message}
                };
            }
            return result;
        }

        #endregion

        #region Exam
        public Registration GetRegistrationForExams(string registrationId)
        {
            OESData db = new OESData();
            return db.Registrations.Include(r => r.Course).Include(a=>a.Chapters)
                .Include(r=> r.Semester)
                .Include(r => r.Exams)
                .FirstOrDefault(r => r.RegistrationId.Equals(registrationId, StringComparison.OrdinalIgnoreCase));
        }

        public Registration GetRegistrationForAllExams(string CourseID)
        {
            OESData db = new OESData();

            return db.Registrations.Include(r => r.Course).Where(q => q.CourseId == CourseID)
                .Include(r => r.Semester)
                .Include(r => r.Exams).FirstOrDefault();
                //.FirstOrDefault(r => r.RegistrationId.Equals(registrationId, StringComparison.OrdinalIgnoreCase));
        }

        public Registration GetRegistrationForAllExamsResults(string CourseID)
        {
            OESData db = new OESData();


            var reg = (from Reg in db.Registrations
                      join course in db.Courses on Reg.CourseId equals course.CourseId
                      where course.CourseId == CourseID
                      join sem in db.Semesters on Reg.SemesterId equals sem.SemesterId
                      //join exa in db.Exams on Reg.RegistrationId equals exa.RegistrationId
                      join exaVer in db.ExamVersions on Reg.RegistrationId equals exaVer.RegistrationId
                      join Ques in db.QuestionsVersions on exaVer.ExamVersionId equals Ques.QuestionVersionId
                      select Reg).FirstOrDefault();

            //db.ExamVersions.Include(r => r.Questions).Include()
            //return db.Registrations.Include(r => r.Course).Where(q => q.CourseId == CourseID)
            //    .Include(r => r.Semester).Include(r => r.ExamVersions).Include(r => r.q)
            //    .Include(r => r.ExamVersions).FirstOrDefault();
            return reg;
            //.FirstOrDefault(r => r.RegistrationId.Equals(registrationId, StringComparison.OrdinalIgnoreCase));
        }
        public Registration GetRegistrationForStudentExams(string registrationId)
        {
            OESData db = new OESData();
            return db.Registrations.Include(r => r.Course)
                .Include(r => r.Semester)
                .Include(r => r.Exams)
                .Include(r => r.Students)
                .Include(r => r.ExamVersions)
                .FirstOrDefault(r => r.RegistrationId.Equals(registrationId, StringComparison.OrdinalIgnoreCase));
        }

        public Registration GetRegistrationForValidExams(string registrationId)
        {
            OESData db = new OESData();
            var reg = db.Registrations.Include(r => r.Course)
                .Include(r => r.Semester)
                .Include(r => r.Exams)
                .FirstOrDefault(r => r.RegistrationId.Equals(registrationId, StringComparison.OrdinalIgnoreCase));
            List<Exam> validExams = new List<Exam>();
            GenerateExamModule module = new GenerateExamModule();
            foreach (var exam in reg.Exams)
            {

                if (module.IsValidExamForGeneration(exam))
                {
                    validExams.Add(exam);
                }
            }
            reg.Exams = validExams;
            return reg;
        }


        public Result<Exam> AddExam(Exam exam)
        {
            var result = new Result<Exam>();
            OESData db = new OESData();
            try
            {
                var errors = ValidateExam(exam);
                if (errors.Count > 0)
                {
                    result.Errors = new List<ResultError>(errors);
                    result.Success = false;
                }
                else
                {
                    db.Exams.Add(exam);
                    db.SaveChanges();
                    db.Dispose();
                    result.Success = true;
                    result.ReturnObject = exam; 
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = exam;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(ex)
                };
            }
            return result;

        }

        private List<ResultError> ValidateExam(Exam exam)
        {
            List<ResultError> errors = new List<ResultError>();
            if (exam.StartDate <= DateTime.Now)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.StartDate, "Exam start date should be in the future."));
            }
            if (exam.EndDate <= exam.StartDate)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.EndDate, "Exam end date should be after the start date."));
            }

            decimal ExamTotalScode = exam.CompleteHighScore * exam.CompleteHigh + exam.CompleteLowScore * exam.CompleteLow + exam.CompleteMediumScore * exam.CompleteMedium + exam.MCQHighScore * exam.MCQHigh + exam.MCQLowScore * exam.MCQLow + exam.MCQMediumScore * exam.MCQMedium + exam.TrueFalseHighScore * exam.TrueFalseHigh + exam.TrueFalseLowScore * exam.TrueFalseLow + exam.TrueFalseMediumScore * exam.TrueFalseMedium;
            if (exam.TotalScore != (float)ExamTotalScode)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.TotalScore, "Exam score dose not equal all questions scores"));
            }
            if (exam.Duration <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.Duration, "Must be greater than 0 in minutes"));
            }

            errors.AddRange(ValidateQuestionsNumber(exam));
            errors.AddRange(ValidateQuestionScore(exam));

            return errors;
        }


        private List<ResultError> ValidateQuestionsNumber(Exam exam)
        {
            var errors = new List<ResultError>();
            int total = exam.MCQHigh + exam.MCQMedium + exam.MCQLow
                + exam.CompleteHigh + exam.CompleteMedium + exam.CompleteLow
                + exam.TrueFalseHigh + exam.TrueFalseMedium + exam.TrueFalseLow;
            if (total < 1)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.MCQHigh, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.MCQMedium, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.MCQLow, "Number of questions should be more than zero."));

                errors.Add(ResultError.AddPropertyError(exam, e => e.CompleteHigh, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.CompleteMedium, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.CompleteLow, "Number of questions should be more than zero."));

                errors.Add(ResultError.AddPropertyError(exam, e => e.TrueFalseHigh, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.TrueFalseMedium, "Number of questions should be more than zero."));
                errors.Add(ResultError.AddPropertyError(exam, e => e.TrueFalseLow, "Number of questions should be more than zero."));
            }
            return errors;
        }

        private List<ResultError> ValidateQuestionScore(Exam exam)
        {
            var errors = new List<ResultError>();
            if (exam.MCQHigh > 0 && exam.MCQHighScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.MCQHighScore, "Enter valid score."));
            }
            if (exam.MCQMedium > 0 && exam.MCQMediumScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.MCQMediumScore, "Enter valid score."));
            }
            if (exam.MCQLow > 0 && exam.MCQLowScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.MCQLowScore, "Enter valid score."));
            }

            if (exam.CompleteHigh > 0 && exam.CompleteHighScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.CompleteHighScore, "Enter valid score."));
            }
            if (exam.CompleteMedium > 0 && exam.CompleteMediumScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.CompleteMediumScore, "Enter valid score."));
            }
            if (exam.CompleteLow > 0 && exam.CompleteLowScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.CompleteLowScore, "Enter valid score."));
            }

            if (exam.TrueFalseHigh > 0 && exam.TrueFalseHighScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.TrueFalseHighScore, "Enter valid score."));
            }
            if (exam.TrueFalseMedium > 0 && exam.TrueFalseMediumScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.TrueFalseMediumScore, "Enter valid score."));
            }
            if (exam.TrueFalseLow > 0 && exam.TrueFalseLowScore <= 0)
            {
                errors.Add(ResultError.AddPropertyError(exam, e => e.TrueFalseLowScore, "Enter valid score."));
            }
            return errors;
        }

        #endregion

    }
}
