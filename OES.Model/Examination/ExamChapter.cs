    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class ExamChapter : BaseEntity 
    {
        public ExamChapter()
        {
            ExamChapterId = GenerateKey();
        }
        [Key]
        public string ExamChapterId { get; set; }
       

        public string ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

        public string ChapterId { get; set; }
        public string ChapterName { get; set; }

    }
}
