using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Server
{
    class Chat
    {
        public delegate void ChatSortDelegate(string str, string plyName);
        public static ChatSortDelegate LocalChatChecks;

        public static void SortMessage(string[] msg)
        {
            string pName = msg[1];

            Player player = Player.GlobalList[pName.ToUpper()];
            switch (msg[2])
            {
                case "G": //Global
                    throw new NotImplementedException();
                    break;
                case "L": //Local
                    SortLocal(msg, pName);
                    break; 
            }
        }

        private static void SortLocal(string[] msg, string pName)
        {
            //Add colons back if split due to server split removal.
            string finString = msg[3];
            if(msg.Length > 4)
                for (int i = 4; i < msg.Length; i++)
                {
                    finString += ":" + msg[i];
                }
            string[] firstWordSplit = finString.Split(' ');
            if (StringHelper.ContainsWordsInSequence(firstWordSplit[0], Spells.SpellInstancer.SpellNames()))
            {
                SendMessageToTile( Spells.SpellInstancer.GetParticleCastString(firstWordSplit[0]) + finString, Player.GlobalList[pName.ToUpper()]);
            }
            else
            SendMessageToTile(finString, Player.GlobalList[pName.ToUpper()]);
            //Checks
            if (LocalChatChecks != null)
                LocalChatChecks.Invoke(finString, pName);
            //Duck
            if (StringHelper.ContainsWordsInSequence(finString, "SUMMON DUCK"))
            {
                SendAnonymousMessageToTile(msg[1] + " summons a duck.", Player.GlobalList[msg[1].ToUpper()]);
            }
            //NPC
            if (firstWordSplit.Length > 1)
                switch (firstWordSplit[0].ToUpper())
                {
                    case "HI":
                    case "GREETINGS":
                    case "HELLO":
                    case "YO":
                    case "SUP":
                    case "HEY":
                    case "HOWDY":
                    case "MORNING":
                    case "AFTERNOON":
                    case "EVENING":
                    case "ALOHA":
                    case "BONJOUR":
                        if (Player.GlobalList[pName.ToUpper()].CurrentTile.ContainsNPC(firstWordSplit[1]))
                        {
                            PlayerMessage ms = new PlayerMessage();
                            ms.PlayerName = pName;
                            ms.Message = "TALK";
                            Player.GlobalList[pName.ToUpper()].CurrentTile.GetNPCFromResponsiveName(firstWordSplit[1]).CommandQueue.Enqueue(ms);
                        }
                        break;
                }
            

            if (Player.GlobalList[pName.ToUpper()].InConversation)
            {
                if (Player.GlobalList[pName.ToUpper()].CurrentConversation != null)
                {
                    PlayerMessage pm = new PlayerMessage();
                    pm.Message = finString;
                    pm.PlayerName = pName.ToUpper();
                    Player.GlobalList[pName.ToUpper()].CurrentConversation.MessageQueue.Enqueue(pm);
                }
            }
        }

        public static void SendMessageToTile(string msg, Player player)
        {            
            foreach(Player ply in player.CurrentTile.PlayerList.Values)
            {
                NetOutgoingMessage outMsg = Server.NetServer.CreateMessage();
                outMsg.WritePadBits();
                outMsg.Write("`p"+player.Name + ":`w " + msg);
                Server.NetServer.SendMessage(outMsg, ply.Connection, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public static void SendAnonymousMessageToTile(string msg, Player player)
        {
            foreach (Player ply in player.CurrentTile.PlayerList.Values)
            {
                NetOutgoingMessage outMsg = Server.NetServer.CreateMessage();
                outMsg.WritePadBits();
                outMsg.Write(msg);
                Server.NetServer.SendMessage(outMsg, ply.Connection, NetDeliveryMethod.ReliableOrdered);
            }
        }

        public static void SendMessageToPlayer(string pName, string message)
        {
            NetOutgoingMessage outMsg = Server.NetServer.CreateMessage();
            outMsg.WritePadBits();
            outMsg.Write(message);

            Server.NetServer.SendMessage(outMsg, Player.GlobalList[pName.ToUpper()].Connection, NetDeliveryMethod.ReliableOrdered);
            
        }
    }
}
