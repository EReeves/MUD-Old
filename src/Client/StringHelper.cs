using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class StringHelper
    {
        public static bool ContainsWordsInSequence(string source, string comparison)
        {
            string[] sSplit = source.Split(' ');
            string[] cSplit = comparison.Split(' ');

            for (int i = 0; i < cSplit.Length; i++)
            {
                if (sSplit.Length < cSplit.Length)
                    return false;
                if (sSplit[i] == cSplit[i])
                    continue;
                else
                    return false;
            }
            return true;
        }
    }
}
