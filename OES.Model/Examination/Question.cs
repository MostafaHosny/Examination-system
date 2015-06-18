using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Question : BaseEntity
    {
        [Key]
        public string QuestionId { get; set; }
        public Question()
        {
            QuestionId = GenerateKey();
        }

        [Required]
        [Display(Name="Text")]
        public string QuestionText { get; set; }

        [Required]
        public string ChapterId { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        public List<Answer> Answers { get; set; }

        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }

        [Required]
        public QuestionDifficulty Difficulty { get; set; }
    }

    public enum QuestionType
    {
        [Display(Name="MCQ")]
        MCQ,
        [Display(Name="True/False")]
        TrueFalse,
        [Display(Name="Complete")]
        Complete
    }

    public enum QuestionDifficulty{
        Low,
        Medium,
        High

    }

  
}
