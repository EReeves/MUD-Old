using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Tiles.Diner
{
    class ITile : Tile
    {
        public ITile() : base(new Vec2(1,1))
        {
            ClientMapOffset = new Vec2(15, 45);
            StartDescription = 
            "You awaken to find yourself seated in one of the booths of a diner, wallet, `bwatch`w and keys still with you. " +
            "A quick survey of the diner shows two young men surrounding a `bjukebox`w, a `bteenage girl`w in the booth adjacent to you; " +
            "she seems to have fallen asleep and a `bbartender`w who seems to be beckoning you toward him. There is a `bDoor`w that seems to exit to the `bStreet`w.";

            Description =
            "A `bplate`w of half eaten cinnamon toast and bananas with cream sits on the table in front of you. " +
            "Your `bseat`w is sticky and smells of soda. You are seated next to a frosted but not opaque `bwindow`w. " +
            "A `bteenage girl`w snores aggressively to your left. " +
            "Two young men click their fingers slightly out of time to a song playing on the `bjukebox`w. " +
            "The `bbartender`w seems to have something important to tell you.";

            Item teenager = new Item(this, "Teenage Girl");
            teenager.description = "There's no waking her.";
            teenager.pickupFailedString = new string[] { "That may be considered a crime." };
            teenager.responsiveNames = new string[] { "TEENAGE GIRL", "GIRL", "TEENAGER", "TEENAGE", "FEMALE" };

            Item jukebox = new Item(this, "Jukebox");
            jukebox.description = "The Jukebox is playing your jam!";
            jukebox.responsiveNames = new string[] { "JUKEBOX", "MUSIC", "MEN" };

            Item window = new Item(this, "Window");
            window.description = "You look out the window, the faint outline of a sprawling city shows through the frosted glass.";

            Item blueTable = new Item(this, "Blue Table");
            blueTable.description = "It's a blue Table. Probably with gum stuck to its underside.";
            blueTable.responsiveNames = new string[] { "BLUE TABLE", "TABLE" };

            Item chair = new Item(this, "Chair");
            chair.description = "The red, plush seat feels sticky to the touch but thankfully it smells like soda so at least you haven't soiled yourself.";
            chair.responsiveNames = new string[] { "CHAIR", "SEAT", "CUSHION" };

            Item plate = new Item(this, "Plate");
            plate.description = "A plate of half eaten cinnamon toast and bananas with cream sits on the table in front of you, looks like its been there a few days.";
            plate.responsiveNames = new string[] { "PLATE", "TOAST", "BANANAS", "BANANA", "CINNAMON", "CREAM", "DESSERT" };

            Item Door = new Item(this, "Door");
            Door.description = "It's a door and it exits to the street.";
            Door.responsiveNames = new string[] { "DOOR", "STREET" };
            Door.CheckString += (PlayerMessage pm) =>
            {
                string[] comparison = new string[] { "USE DOOR", "GOTO STREET", "GO TO STREET" };
                if (StringHelper.ContainsWordsInSequence(pm.Message, comparison))
                {
                    Server.streetTile.PlayerEnter(pm.PlayerName);
                }
            };
            NPC bartender = new NPC("Frank");
            bartender.ResponsiveNames = new string[] { "FRANK", "BARTENDER" };
            bartender.ConversationChecks += (PlayerMessage pm) =>
            {
                if (StringHelper.ContainsWordsInSequence(pm.Message, "TEST"))
                {
                    bartender.SendMessage("TEST WORKS!", Player.GlobalList[pm.PlayerName.ToUpper()]);
                }
            };
            this.AddNPC(bartender);
        }
    }
}
