using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OES
{

    public enum inststatus
    {
        Active,
        Inactive
    }
}
namespace OES.Model.Users
{
    public class Instructor : User
    {
        public Instructor(): base()
        {
            Role = UserRole.Instructor;
        }
        public List<Registration> Registrations { get; set; }

        public inststatus InstructorStatus { get; set; }

    }
}
