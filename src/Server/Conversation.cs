using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Conversation
    {
        public Player PlayerI;
        public NPC NPC;

        public Queue<PlayerMessage> MessageQueue = new Queue<PlayerMessage>();
        public NPC.CommandDelegate CheckMessage;

        public Conversation(string playerName, NPC npc)
        {
            PlayerI = Player.GlobalList[playerName];
            NPC = npc;
            PlayerI.InConversation = true;
            PlayerI.CurrentConversation = this;
            CheckMessage = npc.ConversationChecks;
            SendNPCGreeting();
            Server.OnUpdate += () => { if (PlayerI.InConversation) Update(); };
            RegisterGoodbyeChecks();
        }

        private void Update()
        {
            CheckMessageQueue();              
        }

        private void CheckMessageQueue()
        {
            //If MessageQueue contains message/s, invoke checkmessage.
            if (MessageQueue.Count == 0)
                return;
            PlayerMessage pm = MessageQueue.Dequeue();
            CheckMessage.Invoke(pm);
        }

        private void SendNPCGreeting()
        {
            if (NPC.greetings == null) //Send a generic greeting. Return;
                { Chat.SendAnonymousMessageToTile("`n" + NPC.Name + "`w: Hello " + PlayerI.Name + ".", PlayerI); return; }

            string greeting = "";
            if (NPC.greetings.Length == 1)
            {
                greeting = NPC.greetings[0].Replace("-p-", PlayerI.Name); //Add any instances of the players name.
                NPC.SendMessage(greeting, PlayerI);
                return;
            }
            else
            {
                int index = Dice.Random.Next(0, greeting.Length - 1);
                greeting = NPC.greetings[index].Replace("-p-", PlayerI.Name); //Again.
                NPC.SendMessage(greeting, PlayerI);
            }
        }

        private void SendNPCFarewell()
        {
            if (NPC.farewells == null)
            {
                NPC.SendMessage("Goodbye " + PlayerI.Name +".",PlayerI);
                return;
            }
            string farewell = "";
            if (NPC.farewells.Length == 1)
            {
                farewell = NPC.farewells[0].Replace("-p-", PlayerI.Name);
                NPC.SendMessage(farewell, PlayerI);
                return;
            }
            else
            {
                int index = Dice.Random.Next(0,NPC.farewells.Length-1);
                farewell = NPC.farewells[index].Replace("-p-",PlayerI.Name);
                NPC.SendMessage(farewell, PlayerI);
            }
        }

        private void RegisterGoodbyeChecks()
        {
            CheckMessage += (PlayerMessage pm) =>
            {
                string[] comparisonString = new string[]
                {
                    "BYE", "GOODBYE", "GOOD BYE", "CYA", "SEE YOU", "LATER", "CIAO",
                    "SO LONG", "AU REVOIR", "ADIOS", "GODSPEED", "FAREWELL", "SEE YOU LATER"
                };
                if (StringHelper.ContainsWordsInSequence(pm.Message, comparisonString))
                {
                    //End Conversation;
                    PlayerI.InConversation = false;
                    SendNPCFarewell();
                }
            };
        }

    }
}
