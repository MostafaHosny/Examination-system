using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Chapter : BaseEntity
    {
        public Chapter() {
            ChapterId = GenerateKey();
        }
        public string ChapterId { get; set; }

        [Required]
        public int Number {get; set;}

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Overview { get; set; }

        [Required]
        public string RegistrationId { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration Registration  { get; set; }

        public List<Question> Questions { get; set; }


    }
}
