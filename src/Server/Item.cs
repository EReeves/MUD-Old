using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Item
    {

        public static Dictionary<string, Item> GlobalList = new Dictionary<string, Item>();

        public string Name;
        public string shortDescription;
        public string description;
        public int amount = 1;
        public string suffix = "s";
        public bool pickup = false;
        public string[] pickupFailedString;
        public string[] responsiveNames;
        private Tile currentTile;

        //Command vars
        public Queue<PlayerMessage> CommandQueue = new Queue<PlayerMessage>();
        public delegate void CommandDelegate(PlayerMessage pm);
        public CommandDelegate CheckString;

        public Item(Tile tile, string name)
        {
            Name = name;
            responsiveNames = new string[1] { name.ToUpper() };
            GlobalList.Add(Name.ToUpper(), this);
            currentTile = tile;
            currentTile.ItemList.Add(Name.ToUpper(), this);

            Server.OnUpdate += () =>
            {
                if (currentTile.PlayerList.Count > 0)
                {
                    Update();
                }
            };
            AddCommandChecks();
        }

        public void Update()
        {
            CommandQueueDequeue();
        }

        private void CommandQueueDequeue()
        {
            if (CommandQueue.Count < 1)
                return;
            PlayerMessage msg = CommandQueue.Dequeue();
            CheckString(msg);
        }

        //Default checks included with all items.
        private void AddCommandChecks()
        {
            CheckString += (PlayerMessage pm) =>
            {
                if (pm.Message.Contains("PICK UP"))
                {
                    if (!pickup)
                    {
                        if (pickupFailedString == null || pickupFailedString.Count() < 1)
                        {
                            Chat.SendMessageToPlayer(pm.PlayerName, "You can't pick that up.");
                            return;
                        }
                        int index = Dice.Random.Next(0, pickupFailedString.Count() - 1);
                        Chat.SendMessageToPlayer(pm.PlayerName, pickupFailedString[index]);
                        return;
                    }  
                    Player.GlobalList[pm.PlayerName.ToUpper()].Inventory.ItemList.Add(this);
                    if(amount > 1)
                        Chat.SendMessageToPlayer(pm.PlayerName, "You picked up `b" + amount + " " + Name + suffix);
                    else
                        Chat.SendMessageToPlayer(pm.PlayerName, "You picked up a`b " + Name);
                    currentTile.ItemList.Remove(Name.ToUpper());
                }
            };

            CheckString += (PlayerMessage pm) =>
            {
                bool continueInspect = false;
                foreach (string name in responsiveNames)
                {
                    List<string> comparison = new List<string>();// new string[] { "INSPECT " + Name, "EXAMINE " + Name };
                    foreach (string s in responsiveNames)
                    {
                        comparison.Add("INSPECT " + s);
                        comparison.Add("EXAMINE " + s);
                    }
                    if (StringHelper.ContainsWordsInSequence(pm.Message, comparison.ToArray()))
                    {
                        continueInspect = true;
                    }
                }
                if (!continueInspect)
                    return;

                Chat.SendMessageToPlayer(pm.PlayerName, description);
            };
        }
     
    }
}
