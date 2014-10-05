using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Server
{
    class Server
    {
        public static Dictionary<Vec2, Tile> Tiles = new Dictionary<Vec2, Tile>();
        public static Random Random = new Random(DateTime.Now.Millisecond);
        public delegate void ServerDelegate();
        public static event ServerDelegate OnUpdate;
        public static NetServer NetServer;
        public static bool ServerClose = false;

        //DEBUG//////////////////////////////////////////////
        public static Tiles.Diner.ITile debugTile = new Tiles.Diner.ITile();
        public static Tiles.Street.ITile streetTile = new Tiles.Street.ITile();
        /////////////////////////////////////////////////////////
        
        static void Main(string[] args)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("SF");
            config.Port = 14040;
          //  config.DisableMessageType(NetIncomingMessageType.ConnectionApproval | NetIncomingMessageType.ConnectionLatencyUpdated | NetIncomingMessageType.DebugMessage | NetIncomingMessageType.DiscoveryRequest | NetIncomingMessageType.DiscoveryResponse | NetIncomingMessageType.Error | NetIncomingMessageType.ErrorMessage | NetIncomingMessageType.NatIntroductionSuccess | NetIncomingMessageType.Receipt | NetIncomingMessageType.StatusChanged | NetIncomingMessageType.UnconnectedData | NetIncomingMessageType.VerboseDebugMessage | NetIncomingMessageType.WarningMessage);
            NetServer = new NetServer(config);
            NetServer.Start();
            Console.WriteLine("Server started on port " + NetServer.Port + ".");
            IncomingMessageHandler incomingMessageHandler = new IncomingMessageHandler();
            incomingMessageHandler.Start();

            //DEBUG/////////////////////////////////////////////////
            Tile.DefaultTile = debugTile;
            Spells.SpellInstancer sI = new Spells.SpellInstancer();
            ////////////////////////////////////////////////////
            
            while (!ServerClose)
            {
                if(OnUpdate != null)
                    OnUpdate.Invoke();
            }
        }
    }
}
