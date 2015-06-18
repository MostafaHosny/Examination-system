using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model
{
    public class BaseEntity
    {

        protected string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        public string GeneratePassword()
        {
            Random rnd = new Random();
            return rnd.Next(1000,10000).ToString();
        }
    }
}
