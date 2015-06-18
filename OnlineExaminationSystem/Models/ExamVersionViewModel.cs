using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class ExamVersionViewModel
    {
        public static ExamVersionViewModel BuildViewModel(ExamVersion version)
        {
            ExamVersionViewModel model = new ExamVersionViewModel();
            model.Version = version;
            model.UserScore = 0;
            model.TotalScore = 0;
            model.Duration = version.Duration;
            foreach (var q in version.Questions)
            {
                model.UserScore += model.GetUserScore(q);
                model.TotalScore += model.GetQuestionScore(q);
            }
            model.ExamVersionId = version.ExamVersionId;
            model.MCQQuestions = version.Questions.Where(q => q.Type.Equals(QuestionType.MCQ)).ToList();
            model.CompleteQuestions = version.Questions.Where(q => q.Type.Equals(QuestionType.Complete)).ToList();
            model.TrueFalseQuestions = version.Questions.Where(q => q.Type.Equals(QuestionType.TrueFalse)).ToList();

            return model;
        }
        public ExamVersion Version { get; set; }
        public string ExamVersionId { get; set; }
        public decimal UserScore { get; set; }
        public decimal TotalScore { get; set; }

        public int Duration { get; set; }
        public List<QuestionVersion> MCQQuestions { get; set; }
        public List<QuestionVersion> CompleteQuestions { get; set; }
        public List<QuestionVersion> TrueFalseQuestions { get; set; }

        public decimal GetQuestionScore(QuestionVersion question)
        {
            switch (question.Type)
            {
                case QuestionType.MCQ: switch (question.Difficulty)
                    {
                        case QuestionDifficulty.High: return Version.MCQHighScore;
                        case QuestionDifficulty.Medium: return Version.MCQMediumScore;
                        case QuestionDifficulty.Low: return Version.MCQLowScore;
                    }
                    return 0;
                case QuestionType.Complete: switch (question.Difficulty) {
                    case QuestionDifficulty.High: return Version.CompleteHighScore;
                    case QuestionDifficulty.Medium: return Version.CompleteMediumScore;
                    case QuestionDifficulty.Low: return Version.CompleteLowScore;
                }
                    return 0;

                case QuestionType.TrueFalse: switch (question.Difficulty)
                    {
                        case QuestionDifficulty.High: return Version.TrueFalseHighScore;
                        case QuestionDifficulty.Medium: return Version.TrueFalseMediumScore;
                        case QuestionDifficulty.Low: return Version.TrueFalseLowScore;
                    }
                    return 0;
                default: return 0;
            }
        }

        public decimal GetUserScore(QuestionVersion question)
        {
            if (question.Answers.Any(a => a.IsCorrectAnswer && a.IsThisUserAnswer))
            {
                return GetQuestionScore(question);
            }
            return 0;
        }
    }
}