using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Exam : BaseEntity
    {
        public Exam()
        {
            ExamId = GenerateKey();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public string ExamId { get; set; }

        [Required]
        [Display(Name = "Start Date & Time")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name="End Date & Time")]
        public DateTime EndDate { get; set; }


        [Required]
        public int Duration { get; set; }

        [Required]
        public type_ ExamType { set; get; }

        [Required]
        public float TotalScore { get; set; }
        

        #region MCQ

        [Required]
        [Display(Name = "Number OF High Question")]
        public int MCQHigh { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal MCQHighScore { get; set; }
        [Required]
        [Display(Name = "Number OF Medium Question")]
        public int MCQMedium { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal MCQMediumScore { get; set; }
        [Required]
        [Display(Name = "Number OF Low  Question")]
        public int MCQLow { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal MCQLowScore { get; set; }

        #endregion


        #region Complete

        [Required]
        [Display(Name = "Number OF High Question")]
        public int CompleteHigh { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal CompleteHighScore { get; set; }
        [Required]
        [Display(Name = "Number OF Medium Question")]
        public int CompleteMedium { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal CompleteMediumScore { get; set; }
        [Required]
        [Display(Name = "Number OF Low Question")]
        public int CompleteLow { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal CompleteLowScore { get; set; }

        #endregion

        #region TrueFalse

        [Required]
        [Display(Name = "Number OF High Question")]
        public int TrueFalseHigh { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal TrueFalseHighScore { get; set; }
        [Required]
        [Display(Name = "Number OF Medium Question")]
        public int TrueFalseMedium { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal TrueFalseMediumScore { get; set; }
        [Required]
        [Display(Name = "Number OF Low Question")]
        public int TrueFalseLow { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal TrueFalseLowScore { get; set; }

        #endregion


        [ForeignKey("RegistrationId")]
        public Registration Registration { get; set; }
        public string RegistrationId { get; set; }
        public List<ExamVersion> ExamVersions { get; set; }
        public List<ExamChapter> ExamChapters { get; set; }
    
        public enum type_
        {
            Final,
            Midterm,
            Quiz
        }
    }
}
