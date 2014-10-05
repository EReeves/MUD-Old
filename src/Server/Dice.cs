using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Dice
    {
        public static Random Random = new Random(DateTime.Now.Millisecond);

        public static int D20(int addition = 0)
        {
            return Random.Next(1, 20) + addition;
        }
    }
}
