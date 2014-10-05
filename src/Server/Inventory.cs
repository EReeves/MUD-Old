using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Inventory
    {
        private Player player;
        public List<Item> ItemList = new List<Item>();

        public Inventory(Player _player)
        {
            player = _player;
        }

        public void DisplayInventory()
        {
            if (ItemList.Count < 1)
            { Chat.SendMessageToPlayer(player.Name, "You have no items."); return; }

            string itemList = "`rInventory `b \n";
            foreach (Item i in ItemList)
            {
                itemList += "\t\t" + i.Name + "\n";
            }

            Chat.SendMessageToPlayer(player.Name, itemList);
        }

    }
}
