using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    class SortInput
    {
        private static List<string> commandList = new List<string>();

        static SortInput()
        {
            string[] str = File.ReadAllLines("Data/commands");
            commandList.AddRange(str);
        }

        public static void SortInputString(string str)
        {
            if (LocalCommand(str))
                return;

            string[] splitStr = str.Split(' ');
            foreach (string st in commandList)
            {
                if (splitStr[0].ToUpper() == st.Split(' ')[0] && str.ToUpper().Contains(st))
                {
                    Program.CNetwork.SendCommandToServer(Program.PlayerName, str);
                    return;
                }
            }
            Program.CNetwork.SendLocalMessageToServer(Program.PlayerName, str);
        }

        private static bool LocalCommand(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                Program.CConsole.WriteLine("You say nothing.");
                return true;
            }

            string s = str.ToUpper();

            if (StringHelper.ContainsWordsInSequence(s, "WATCH") ||
                StringHelper.ContainsWordsInSequence(s, "CHECK WATCH") ||
                StringHelper.ContainsWordsInSequence(s, "CHECK TIME") ||
                StringHelper.ContainsWordsInSequence(s, "TIME"))
            {
                Program.CConsole.WriteLine("Your watch reveals that it is `o" + System.DateTime.Now.ToLongTimeString() + "`w");
            }
            return false;
        }
    }
}
