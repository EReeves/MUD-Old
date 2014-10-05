using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// Command string checks can be added to CheckString.
    class NPC
    {
        public static Dictionary<string,NPC> GlobalList = new Dictionary<string,NPC>();

        public Tile Tile;
        public string Name { get; set; }
        public string shortDescription { get; set; }
        public string description { get; set; }
        public string[] conversation { get; set; }
        public string[] greetings { get; set; }
        public string[] farewells { get; set; }
        public string[] aggression { get; set; }
        public string[] idle { get; set; }
        public string[] weapons { get; set; }
        public string[] death { get; set; }
        public Item[] Items; //TODO: Implement dropping of these.
        public string[] ResponsiveNames;

        //Combat
        public int Health = 100;
        public bool IsMelee = false;
        public int GlobalDamageResistance = 1;
        public int DodgeChance = 10;
        public List<Combat.DamageType> DamageTypeResistances = new List<Combat.DamageType>();
        public List<Combat.Attack> Attacks = new List<Combat.Attack>();
        
        //Comand vars
        public Queue<PlayerMessage> CommandQueue = new Queue<PlayerMessage>();
        public delegate void CommandDelegate(PlayerMessage pm);
        public CommandDelegate CheckString;
        public CommandDelegate ConversationChecks;

        public NPC(string name)
        {
            Name = name;
            ResponsiveNames = new string[] { name };
            GlobalList.Add(name.ToUpper(), this);

            AddCommandChecks();

            Server.OnUpdate += () =>
            {
                if (Tile.PlayerList.Count > 0)
                {
                    Update();
                }
            };
        }

        protected virtual void AddCommandChecks()
        {
            CheckString += (PlayerMessage pm) =>
            {
                if (description != null && pm.Message.ToUpper().Contains("INSPECT"))
                    Chat.SendMessageToPlayer(pm.PlayerName, description);
            };

            CheckString += (PlayerMessage pm) =>
            {
                if (pm.Message.ToUpper().Contains("TALK"))
                {
                    Conversation convo = new Conversation(pm.PlayerName.ToUpper(),this);
                }
            };
        }

        public void Update()
        {
            SortQueue();
        }

        private void SortQueue()
        {
            if (CommandQueue.Count < 1)
                return;
            PlayerMessage pm = CommandQueue.Dequeue();
            if(CheckString != null)
                CheckString.Invoke(pm);
        }

        public void SendMessage(string str, Player ply)
        {
            Chat.SendAnonymousMessageToTile("`n" + Name + "`w: " + str, ply);
        }
    }
}
