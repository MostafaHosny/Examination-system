using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Users
{
    public class Admin : User
    {
        public Admin(): base()
        {
            Role = UserRole.Admin;
        }
    }
}
