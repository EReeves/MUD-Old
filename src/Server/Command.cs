using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    sealed class Command
    {
        //This is going to be ugly.
        public static void Sort(string[] splitStr)
        {

            string message = splitStr[2].ToUpper();
            //Movement
            switch (message)
            {
                case "NORTH": case "GO NORTH":
                    Tile.PlayerMove(new Vec2(0, -1), splitStr[1]); return;
                case "EAST": case "GO EAST":
                    Tile.PlayerMove(new Vec2(1, 0), splitStr[1]); return;
                case "SOUTH": case "GO SOUTH":
                    Tile.PlayerMove(new Vec2(0, 1), splitStr[1]); return;
                case "WEST": case "GO WEST":
                    Tile.PlayerMove(new Vec2(-1, 0), splitStr[1]); return;
                case "JUMP":
                    Chat.SendMessageToPlayer(splitStr[1], "Well Done."); return;
            }

            //Attack Commands
            if (message.Contains("TEST ATTACK"))
            {
                string[] split = message.Split(' ');
                if (!NPC.GlobalList.ContainsKey(split[2]))
                    return;
                Combat.AttackNPC(splitStr[1].ToUpper(),split[2] , new Combat.Attack());
                return;
            }

            //Chat Commands
            if (message.Length > 2 && message.Substring(0,3) =="/ME")
            { Chat.SendAnonymousMessageToTile(splitStr[1] + " " + splitStr[2].Substring(4, splitStr[2].Length - 4), Player.GlobalList[splitStr[1].ToUpper()]); return; }

            if (StringHelper.ContainsWordsInSequence(message, "SAY"))
            {
                string printString = "";
                printString += splitStr[2].Substring(3);
                if (splitStr.Length > 3)
                {
                    for (int i = 0; i < splitStr.Length; i++)
                    {
                        printString += " " + splitStr[i];
                    }
                }
                Chat.SendMessageToTile(printString, Player.GlobalList[splitStr[1].ToUpper()]);
            }

            //Item Commands
            if (StringHelper.ContainsWordsInSequence(message, "INVENTORY"))
            {
                Player.GlobalList[splitStr[1].ToUpper()].Inventory.DisplayInventory();
            }

            if (StringHelper.ContainsWordsInSequence(message, "PLAYERS"))
            {
                if(Player.GlobalList.Count == 1)
                    Chat.SendMessageToPlayer(splitStr[1], "You are the only player online.");
                else
                    Chat.SendMessageToPlayer(splitStr[1], "There are `o" + Player.GlobalList.Count + "`w players online.");
            }

            if (StringHelper.ContainsWordsInSequence(message, "NPC"))
            {
                NPC[] npcArray = Player.GlobalList[splitStr[1].ToUpper()].CurrentTile.NpcList.ToArray();
                if (npcArray.Count() == 0)
                    Chat.SendMessageToPlayer(splitStr[1], "There are no NPC's in this area.");
                else
                {
                    string printString = "`oNPC/s in the area:\n`w";
                    foreach (NPC n in npcArray)
                    {
                        printString += "`n"+n.Name+"`w ";
                    }
                    Chat.SendMessageToPlayer(splitStr[1], printString);
                }

            }

            if (StringHelper.ContainsWordsInSequence(message, "LIST"))
            {
                Tile tile = Player.GlobalList[splitStr[1].ToUpper()].CurrentTile;
                string printString = "`oInteractive objects and NPC's in the area:`w\n";
                foreach (NPC n in tile.NpcList)
                {
                    printString += "\t`n" + n.Name + "`w\n";
                }
                foreach (Item i in tile.ItemList.Values)
                {
                    printString += "\t`b" + i.Name + "`w\n";
                }
                Chat.SendMessageToPlayer(splitStr[1], printString);
            }

            if (StringHelper.ContainsWordsInSequence(message,"PICK UP"))
            {
                string[] split = message.Split(' ');
                string name = "";
                for (int i = 2; i < split.Length; i++)
                {
                    name += split[i] + ' ';
                }
                SendCommandToTileItem(message,name.ToUpper(), splitStr[1]); 
                return; 
            }

            if (StringHelper.ContainsWordsInSequence(message, "USE") || StringHelper.ContainsWordsInSequence(message, "GOTO"))
            {
                Player.GlobalList[splitStr[1].ToUpper()].CurrentTile.SendItemCommandToTile(message, splitStr[1].ToUpper());
            }

            if (StringHelper.ContainsWordsInSequence(message, "INSPECT") || StringHelper.ContainsWordsInSequence(message, "EXAMINE"))
            {
                string[] inspectSplit = message.Split();
                if (inspectSplit.Count() > 1 && inspectSplit[1] == "AREA")
                { 
                    Chat.SendMessageToPlayer(splitStr[1], Player.GlobalList[splitStr[1].ToUpper()].CurrentTile.Description);
                    Chat.SendMessageToPlayer(splitStr[1], "`oPlayers`w");
                    Player[] playerArray = Player.GlobalList[splitStr[1].ToUpper()].CurrentTile.PlayerList.Values.ToArray();
                    for (int i = 0; i < playerArray.Count(); i++)
                    {
                        if (i > 6)
                        {
                            Chat.SendMessageToPlayer(splitStr[1], "\t+ " + (playerArray.Count() - 6).ToString() + " more players.");
                        }
                        Chat.SendMessageToPlayer(splitStr[1], "\t`p"+playerArray[i].Name+"`w");
                    }
                    if(Player.GlobalList[splitStr[1].ToUpper()].CurrentTile.PlayerList.Count > 6)
                    return;
                }
                Player.GlobalList[splitStr[1].ToUpper()].CurrentTile.SendItemCommandToTile(message, splitStr[1].ToUpper());
            }
        }

        private static void SendCommandToTileItem(string msg, string itemName, string pName)
        {
            if (itemName[itemName.Length - 1] == ' ')
                itemName = itemName.Substring(0, itemName.Length-1);
            PlayerMessage plym = new PlayerMessage()
            {
                Message = msg,
                PlayerName = pName
            };
            Tile tile = Player.GlobalList[pName.ToUpper()].CurrentTile;

            if (tile.ItemList.ContainsKey(itemName))
            {
                tile.ItemList[itemName].CommandQueue.Enqueue(plym);
            }
            else
            {
                Chat.SendMessageToPlayer(pName, "There is no such thing silly.");
            }
        }
    }
}
