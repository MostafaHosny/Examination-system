using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Modules.Common
{
    public static class RandomList<T>
    {
        public static List<T> Random(List<T> lst)
        {
            List<T> rndLst = new List<T>();
            List<int> selectedRandom = new List<int>();
            for (int i = 0; i < lst.Count; i++)
            {
                Random rand = new Random();
                int random = rand.Next(0, lst.Count);
                while (selectedRandom.Contains(random))
                {
                    rand = new Random();
                    random = rand.Next(0, lst.Count);
                }
                selectedRandom.Add(random);
                rndLst.Add(lst[random]);
            }
            return rndLst;
        }
    }
}
