using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class AnswerVersion : BaseEntity 
    {
        public AnswerVersion()
        {
            AnswerVersionId = GenerateKey();
        }
        [Key]
        public string AnswerVersionId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name="Answer Text")]
        public string AnswerText { get; set; }

        [Required]
        [Display(Name = "Is This Answer Correct")]
        public bool IsCorrectAnswer { get; set; }


        [Required]
        [Display(Name = "Is This User Answer")]
        public bool IsThisUserAnswer { get; set; }
       
        public string QuestionVersionId { get; set; }

        [ForeignKey("QuestionVersionId")]
        public QuestionVersion QuestionVersion { get; set; }

    }
}
