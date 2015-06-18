using OES.Data;
using OES.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace OnlineExaminationSystem.Extensions
{
    public static class UserSecurityExtenstion
    {

        public static User GetDbUser(this IPrincipal user)
        {
            OESData db = new OESData();
            var dbUser = db.Users.FirstOrDefault(u => u.UserName.Equals(user.Identity.Name, StringComparison.OrdinalIgnoreCase));
            db.Dispose();
            return dbUser;
        }
    }
}