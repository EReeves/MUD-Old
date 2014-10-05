using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Tile
    {
        public static Tile DefaultTile;
        public List<NPC> NpcList = new List<NPC>();
        public Dictionary<string, Player> PlayerList = new Dictionary<string, Player>();
        public Vec2 Coordinate;
        public Dictionary<string, Item> ItemList = new Dictionary<string, Item>();

        public string StartDescription = "TileStartDescription";
        public string Description = "TileDetailedDescription";

        public Vec2 ClientMapOffset = new Vec2(0, 0);

        public Tile(Vec2 coord)
        {
            Coordinate = coord;
            Server.Tiles.Add(coord, this);
        }

        public static void PlayerMove(Vec2 direction, string pName)
        {
            Vec2 nextCoord = Player.GlobalList[pName].CurrentTile.Coordinate + direction;
            if (Server.Tiles.ContainsKey(nextCoord))
            {
                Server.Tiles[nextCoord].PlayerEnter(pName);
            }
            else
            {
                Chat.SendMessageToPlayer(pName, "There is no way to traverse in that direction.");
            }
        }

        public void SendItemCommandToTile(string command, string playerName)
        {
            foreach(Item i in ItemList.Values)
            {
                PlayerMessage pm;
                pm.Message = command;
                pm.PlayerName = playerName;
                i.CommandQueue.Enqueue(pm);
            }
        }

        public NPC GetNPCFromResponsiveName(string NPCName)
        {
            foreach (NPC n in NpcList)
            {
                foreach (string s in n.ResponsiveNames)
                {
                    if (s == NPCName.ToUpper())
                        return n;
                }
            }
            throw new Exception("NPC" + NPCName + "Does not exist.");
        }

        public bool ContainsNPC(string NPCName)
        {
            foreach (NPC n in NpcList)
            {
                foreach (string s in n.ResponsiveNames)
                {
                    if (s == NPCName.ToUpper())
                        return true;
                }
            }
            return false;
        }

        public void PlayerEnter(string pName)
        {
            PlayerExit(pName);
            if(Player.GlobalList[pName.ToUpper()].CurrentTile != null)
                Player.GlobalList[pName.ToUpper()].CurrentTile.PlayerExit(pName);

            PlayerList.Add(pName.ToUpper(), Player.GlobalList[pName.ToUpper()]);
            PlayerList[pName.ToUpper()].CurrentTile = this;

            if (!string.IsNullOrEmpty(Description)) //Send descripton to player console.
                Chat.SendMessageToPlayer(pName, StartDescription);

            Chat.SendMessageToPlayer(pName, "PUD:" + ClientMapOffset.X + ":" + ClientMapOffset.Y);

            
        }

        public void PlayerExit(string pName)
        {
            if(PlayerList.ContainsKey(pName.ToUpper()))
            PlayerList.Remove(pName.ToUpper());
        }

        public void AddNPC(NPC npc)
        {
            npc.Tile = this;
            NpcList.Add(npc);
        }
    }
}
