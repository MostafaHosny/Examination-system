using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Users
{
    public class User : BaseEntity
    {
        public User()
        {
            UserId = GenerateKey();
            Password = GeneratePassword();
        }
        [Key]
        public string UserId { get; set; }

        [Required]
        [Display(Name="User Name")]
        
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name="Id")]
        public int Code { get; set; }


        

        public DateTime? LastVisit { get; set; }

    }

    public enum UserRole
    {
        Admin,
        Instructor,
        Student
    }
}
