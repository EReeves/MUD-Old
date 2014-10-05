using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Tiles.Street
{
    class ITile : Tile
    {
        public ITile() : base(new Vec2(1,2))
        {
            ClientMapOffset = new Vec2(47, 20);
            StartDescription = 
            "You walk out in to the street.";

            Description =
            "No Description Yet.";

            Item dinerEntrance = new Item(this, "Diner");
            dinerEntrance.description = "A Diner.";
            dinerEntrance.CheckString += (PlayerMessage pm) =>
            {
                string[] comparison = new string[] { "USE DINER", "GOTO DINER", "GO TO DINER" };
                if (StringHelper.ContainsWordsInSequence(pm.Message, comparison))
                {
                    Server.debugTile.PlayerEnter(pm.PlayerName);
                }
            };

        }
    }
}
