using OES.Data;
using OES.Model.Examination;
using OES.Modules.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using OES.Model.Users;

namespace OES.Modules.Common
{
    public class GenerateExamModule
    {

        public Result<ExamVersion> GenerateExamVersion(string id)
        {
            OESData db = new OESData();
            var exam = db.Exams
                .Include(e => e.Registration).Include(e=>e.ExamChapters)
                .FirstOrDefault(e => e.ExamId.Equals(id, StringComparison.OrdinalIgnoreCase));
            
            List<string> mychapterIds = new List<string>();

            foreach (var examchapter in exam.ExamChapters)
            {
                var chapter = db.Chapters.Find(examchapter.ChapterId);
                mychapterIds.Add(chapter.ChapterId);
            }
           
            Result<ExamVersion> result = new Result<ExamVersion>();
            var questions = db.Questions.Include(q => q.Answers)
                .Include(q => q.Chapter).
                Where(q => mychapterIds.Contains(q.ChapterId)) //.Contains ()Equals(exam.RegistrationId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            result.Errors = ValidateQuestions(exam, questions);
            result.ReturnObject = new ExamVersion { 
                RegistrationId = exam.RegistrationId
            };

            result.Success = result.Errors.Count < 1;
            if (result.Success)
            {
                ExamVersion version = WrapToExamVersion(exam);
                version.Questions = PickupQuestions(version, questions);
                foreach (var q in version.Questions)
                {
                    q.ExamVersionId = version.ExamVersionId;
                }
               
                db.ExamVersions.Add(version);
                result.ReturnObject = version;
                db.SaveChanges();
            }
            return result;
        }
        public Result<ExamVersion> GenerateExamVersionforInstructor(string id)
        {
            OESData db = new OESData();
            var exam = db.Exams
                .Include(e => e.Registration).Include(e => e.ExamChapters)
                .FirstOrDefault(e => e.ExamId.Equals(id, StringComparison.OrdinalIgnoreCase));

            List<string> mychapterIds = new List<string>();

            foreach (var examchapter in exam.ExamChapters)
            {
                var chapter = db.Chapters.Find(examchapter.ChapterId);
                mychapterIds.Add(chapter.ChapterId);
            }

            Result<ExamVersion> result = new Result<ExamVersion>();
            var questions = db.Questions.Include(q => q.Answers)
                .Include(q => q.Chapter).
                Where(q => mychapterIds.Contains(q.ChapterId)) //.Contains ()Equals(exam.RegistrationId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            result.Errors = ValidateQuestions(exam, questions);
            result.ReturnObject = new ExamVersion
            {
                RegistrationId = exam.RegistrationId
            };

            result.Success = result.Errors.Count < 1;
            if (result.Success)
            {
                ExamVersion version = WrapToExamVersion(exam);
                version.Questions = PickupQuestions(version, questions);
                foreach (var q in version.Questions)
                {
                    q.ExamVersionId = version.ExamVersionId;
                }

                db.ExamVersions.Add(version);
                result.ReturnObject = version;
               // db.SaveChanges();
            }
            return result;
        }

        public Result<ExamVersion> GenerateExamVersionForStudent(string id ,Student studnet)
        {
            OESData db = new OESData();
            var exam = db.Exams
                .Include(e => e.Registration).Include(e => e.ExamChapters)
                .FirstOrDefault(e => e.ExamId.Equals(id, StringComparison.OrdinalIgnoreCase));

            List<string> mychapterIds = new List<string>();

            foreach (var examchapter in exam.ExamChapters)
            {
                var chapter = db.Chapters.Find(examchapter.ChapterId);
                mychapterIds.Add(chapter.ChapterId);
            }

            Result<ExamVersion> result = new Result<ExamVersion>();
            var questions = db.Questions.Include(q => q.Answers)
                .Include(q => q.Chapter).
                Where(q => mychapterIds.Contains(q.ChapterId))
                .ToList();

            result.Errors = ValidateQuestions(exam, questions);
            result.ReturnObject = new ExamVersion
            {
                RegistrationId = exam.RegistrationId,
                StudentId = studnet.UserId
            };
            result.Success = result.Errors.Count < 1;
            if (result.Success)
            {
                ExamVersion version = WrapToExamVersion(exam);
                version.Questions = PickupQuestions(version, questions);
                
                foreach (var q in version.Questions)
                {
                    q.ExamVersionId = version.ExamVersionId;
                }
              
                db.ExamVersions.Add(version);
                version.StudentId = studnet.UserId;
                db.SaveChanges();
                result.ReturnObject = version;
            }
            return result;
        }

        public bool IsValidExamForGeneration(Exam exam)
        {
            OESData db = new OESData();
            var dbExam = db.Exams
                .Include(e => e.Registration)
                .FirstOrDefault(e => e.ExamId.Equals(exam.ExamId, StringComparison.OrdinalIgnoreCase));
            var questions = db.Questions.Include(q => q.Answers)
                .Include(q => q.Chapter).
                Where(q => q.Chapter.RegistrationId.Equals(exam.RegistrationId, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return ValidateQuestions(dbExam, questions).Count < 1;
        }

        private List<ResultError> ValidateQuestions(Exam exam, List<Question> questions)
        {
            List<ResultError> errors = new List<ResultError>();

            errors.AddRange(ValidateMCQQuestions(exam, questions));
            errors.AddRange(ValidateCompleteQuestions(exam, questions));
            errors.AddRange(ValidateTrueFalseQuestions(exam, questions));

            foreach (var q in questions.Where(q => q.Type != QuestionType.TrueFalse && q.Answers.Count < 4))
            {
                errors.Add(new ResultError("", string.Format("Chapter: \"{0}\"Question: \"{1}\" with less than four answers.", q.Chapter.Title, q.QuestionText)));
            }
            foreach (var q in questions.Where(q => q.Answers.Count(a => a.IsCorrectAnswer) < 1))
            {
                errors.Add(new ResultError("", string.Format("Chapter: \"{0}\"Question: \"{1}\" should have at least one correct answer.", q.Chapter.Title, q.QuestionText)));
            }
            return errors;
        }

        #region Validate questions count
        private List<ResultError> ValidateMCQQuestions(Exam exam, List<Question> questions)
        {
            List<ResultError> errors = new List<ResultError>();
            QuestionType qType = QuestionType.MCQ;
            questions = questions.Where(q => q.Type.Equals(qType)).ToList();
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.High)) < exam.MCQHigh)
            {
                errors.Add(new ResultError("", "There is no enough high " + qType + " questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Medium)) < exam.MCQMedium)
            {
                errors.Add(new ResultError("", "There is no enough medium " + qType + " questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Low)) < exam.MCQLow)
            {
                errors.Add(new ResultError("", "There is no enough low " + qType + " questions to generate this exam."));
            }
            return errors;
        }

        private List<ResultError> ValidateCompleteQuestions(Exam exam, List<Question> questions)
        {
            List<ResultError> errors = new List<ResultError>();
            QuestionType qType = QuestionType.Complete;
            questions = questions.Where(q => q.Type.Equals(qType)).ToList();
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.High)) < exam.CompleteHigh)
            {
                errors.Add(new ResultError("", "There is no enough high " + qType + " questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Medium)) < exam.CompleteMedium)
            {
                errors.Add(new ResultError("", "There is no enough medium " + qType + " questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Low)) < exam.CompleteLow)
            {
                errors.Add(new ResultError("", "There is no enough low " + qType + " questions to generate this exam."));
            }
            return errors;
        }

        private List<ResultError> ValidateTrueFalseQuestions(Exam exam, List<Question> questions)
        {
            List<ResultError> errors = new List<ResultError>();
            QuestionType qType = QuestionType.TrueFalse;
            questions = questions.Where(q => q.Type.Equals(qType)).ToList();
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.High)) < exam.TrueFalseHigh)
            {
                errors.Add(new ResultError("", "There is no enough high " + qType + " questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Medium)) < exam.TrueFalseMedium)
            {
                errors.Add(new ResultError("", "There is no enough medium " + qType + " questions to generate this exam."));
            }
            if (questions.Count(q => q.Difficulty.Equals(QuestionDifficulty.Low)) < exam.TrueFalseLow)
            {
                errors.Add(new ResultError("", "There is no enough low " + qType + " questions to generate this exam."));
            }
            return errors;
        }

        #endregion

        private ExamVersion WrapToExamVersion(Exam exam)
        {
            return new ExamVersion()
            {
                MCQHigh = exam.MCQHigh,
                MCQHighScore = exam.MCQHighScore,
                MCQMedium = exam.MCQMedium,
                MCQMediumScore = exam.MCQMediumScore,
                MCQLow = exam.MCQLow,
                MCQLowScore = exam.MCQLowScore,
                CompleteHigh = exam.CompleteHigh,
                CompleteHighScore = exam.CompleteHighScore,
                CompleteMedium = exam.CompleteMedium,
                CompleteMediumScore = exam.CompleteMediumScore,
                CompleteLow = exam.CompleteLow,
                CompleteLowScore = exam.CompleteLowScore,
                TrueFalseHigh = exam.TrueFalseHigh,
                TrueFalseHighScore = exam.TrueFalseHighScore,
                TrueFalseMedium = exam.TrueFalseMedium,
                TrueFalseMediumScore = exam.TrueFalseMediumScore,
                TrueFalseLow = exam.TrueFalseLow,
                TrueFalseLowScore = exam.TrueFalseLowScore,
                StartDate = exam.StartDate,
                EndDate = exam.EndDate,
                Registration = exam.Registration,
                RegistrationId = exam.RegistrationId,
                ExamType = (ExamVersion.type)exam.ExamType,
                ExamId = exam.ExamId,
                Duration = exam.Duration,
                TotalScore = exam.TotalScore

            };
        }
        private List<QuestionVersion> PickupQuestions(ExamVersion exam, List<Question> questions)
        {
            var finalQuestions = new List<QuestionVersion>();
            var tempQuestions = new List<QuestionVersion>();

            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.MCQHigh, QuestionDifficulty.High, QuestionType.MCQ));
            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.MCQMedium, QuestionDifficulty.Medium, QuestionType.MCQ));
            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.MCQLow, QuestionDifficulty.Low, QuestionType.MCQ));

            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.CompleteHigh, QuestionDifficulty.High, QuestionType.Complete));
            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.CompleteMedium, QuestionDifficulty.Medium, QuestionType.Complete));
            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.CompleteLow, QuestionDifficulty.Low, QuestionType.Complete));

            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.TrueFalseHigh, QuestionDifficulty.High, QuestionType.TrueFalse));
            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.TrueFalseMedium, QuestionDifficulty.Medium, QuestionType.TrueFalse));
            tempQuestions.AddRange(PickupTypeOfQuestions(questions, exam.TrueFalseLow, QuestionDifficulty.Low, QuestionType.TrueFalse));

            List<int> selectedRandoms = new List<int>();
            for (int i = 0; i < tempQuestions.Count; i++)
            {
                Random r = new Random();
                int random = r.Next(0, tempQuestions.Count);
                while (selectedRandoms.Contains(random))
                {
                    r = new Random();
                    random = r.Next(0, tempQuestions.Count);
                }
                selectedRandoms.Add(random);
                finalQuestions.Add(tempQuestions[random]);
            }

            return finalQuestions;
        }

        private List<QuestionVersion> PickupTypeOfQuestions(List<Question> questions, int count, QuestionDifficulty diffuclity, QuestionType type)
        {
            var selectedQuestions = new List<QuestionVersion>();
            var qLst = questions.Where(q => q.Difficulty.Equals(diffuclity) && q.Type.Equals(type)).ToList();
            var chapters = qLst.Select(q => q.Chapter).Distinct().OrderBy(c => c.Number).ToList();
            chapters = RandomList<Chapter>.Random(chapters);
            qLst = RandomList<Question>.Random(qLst);
            int i = 0;
            while (i < count) { 
                foreach (var chapter in chapters)
                {
                    var question = qLst.FirstOrDefault(q => q.ChapterId.Equals(chapter.ChapterId, StringComparison.OrdinalIgnoreCase));
                    if (question != null)
                    {
                        qLst.Remove(question);
                        selectedQuestions.Add(WrapToQuestionVersion(question));
                        i++;
                        if (i >= count)
                        {
                            break;
                        }
                    }
                }
            }
            return selectedQuestions;
        }
        

        private QuestionVersion WrapToQuestionVersion(Question question)
        {
            var version = new QuestionVersion()
            {
                Type = question.Type,
                QuestionText = question.QuestionText,
                Difficulty = question.Difficulty,
                Chapter = question.Chapter,
                ChapterId = question.ChapterId

            };
            version.Answers = new List<AnswerVersion>();
            if (question.Type.Equals(QuestionType.TrueFalse))
            {
                version.Answers = question.Answers.Select(a => WrapToAnswerVersion(a)).ToList();
            }
            else
            {
                List<Answer> answers = new List<Answer>();
                answers.AddRange(question.Answers.Where(a => a.IsCorrectAnswer));
                if (answers.Count < 4)
                {
                    answers.AddRange(question.Answers.Where(a => !a.IsCorrectAnswer).Take(4 - answers.Count));
                }
                List<int> selectedRansdom = new List<int>();
                for (int i = 0; i < answers.Count; i++)
                {
                    Random rand = new Random();
                    int random = rand.Next(0, answers.Count);
                    while (selectedRansdom.Contains(random))
                    {
                        rand = new Random();
                        random = rand.Next(0, answers.Count);
                    }
                    selectedRansdom.Add(random);
                    version.Answers.Add(WrapToAnswerVersion(answers[random]));
                }

            }
            return version;
        }

        private AnswerVersion WrapToAnswerVersion(Answer answer)
        {
            return new AnswerVersion()
            {
                AnswerText = answer.AnswerText,
                IsCorrectAnswer = answer.IsCorrectAnswer
            };
        }
    }
}
