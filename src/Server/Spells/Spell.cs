using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Spells
{
    class Spell : Combat.Attack
    {
        public string CastString;
        private int spellWordCount = 0;
        public string particleCastString = "";

        public Spell(string castString)
        {
            CastString = castString;
            spellWordCount = castString.Split(' ').Count();
        }

        public void RegisterCheck()
        {
            Chat.LocalChatChecks += CastCheck;
        }

        public void CastCheck(string str, string plyName)
        {
            if (StringHelper.ContainsWordsInSequence(str, CastString))
            {
                if(str.Split(' ').Count() != spellWordCount + 1)
                    return;
                string targetName = str.Split(' ').Last();

                if(NPC.GlobalList.ContainsKey(targetName.ToUpper()))
                    Combat.PlayerAttack(plyName,targetName,this,true);
                else if(Player.GlobalList.ContainsKey(targetName.ToUpper()))
                    Combat.PlayerAttack(plyName, targetName, this, false);  
            }
        }
    }
}
