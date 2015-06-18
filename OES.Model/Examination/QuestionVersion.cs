using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class QuestionVersion : BaseEntity
    {
        public QuestionVersion() {
            QuestionVersionId = GenerateKey();
        }
        [Key]
        public string QuestionVersionId { get; set; }


        [Required]
        [Display(Name = "Text")]
        public string QuestionText { get; set; }

        [Required]
        public string ChapterId { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        public List<AnswerVersion> Answers { get; set; }

        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }

        [Required]
        public string ExamVersionId { get; set; }

        [ForeignKey("ExamVersionId")]
        public ExamVersion ExamVersion { get; set; }

        [Required]
        public QuestionDifficulty Difficulty { get; set; }
    }
}
