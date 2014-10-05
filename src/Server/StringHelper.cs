using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
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
                if (sSplit[i].ToUpper() == cSplit[i])
                    continue;
                else
                    return false;
            }
            return true;
        }

        public static bool ContainsWordsInSequence(string source, string[] comparison)
        {
            for (int c = 0; c < comparison.Length; c++)
            {
                string[] sSplit = source.ToUpper().Split(' ');
                string[] cSplit = comparison[c].ToUpper().Split(' ');
                bool True = true;

                for (int i = 0; i < cSplit.Length; i++)
                {
                    if (sSplit.Length < cSplit.Length)
                    {
                        True = false;
                        break;
                    }
                    if (sSplit[i].ToUpper() == cSplit[i])
                        continue;
                    else
                        True = false;
                }
                if(True)
                    return true;
            }

            return false;
        }
    }
}
