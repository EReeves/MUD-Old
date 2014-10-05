using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using SFML.Window;
using SFML.Graphics;
using System.IO;
using System.Threading;

namespace Client
{
    class Network
    {
        private NetClient netClient;

        public Network(string ip)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("SF");
            config.Port = 14044;
            config.DisableMessageType(NetIncomingMessageType.ConnectionApproval | NetIncomingMessageType.ConnectionLatencyUpdated | NetIncomingMessageType.DebugMessage | NetIncomingMessageType.DiscoveryRequest | NetIncomingMessageType.DiscoveryResponse | NetIncomingMessageType.NatIntroductionSuccess | NetIncomingMessageType.Receipt | NetIncomingMessageType.StatusChanged | NetIncomingMessageType.UnconnectedData | NetIncomingMessageType.VerboseDebugMessage);
            config.EnableUPnP = true;       
            netClient = new NetClient(config);
            netClient.Start();
            netClient.UPnP.ForwardPort(14044, "MUD Port");

            netClient.Connect(ip, 14040);

            Task waitForConnection = new Task(() =>{
                while (netClient.ConnectionsCount < 1)
                {
                    //WAIT FOR CONNECTION.
                }
            });
            waitForConnection.Start();
            waitForConnection.Wait();
            SendConnectionMessage(Program.PlayerName);
            Program.OnUpdate += CheckIncomingMessages;
        }

        private void CheckIncomingMessages()
        {
            NetIncomingMessage msg = netClient.ReadMessage();
            if (msg != null)
            {
                string message = msg.ReadString();
                if(message.Contains(":"))
                {
                    string[] split = message.Split(':');
                    if (split[0] == "PUD")
                    {
                        Program.Map.mapSpriteOffset = new Vector2f(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]));
                        Program.Map.Alpha = 255;
                        return;
                    }
                }
                Program.CConsole.PureText.Add(message);
                Program.CConsole.WriteLine(message);
            }
        }

        public void SendCommandToServer(string pName, string message)
        {
            NetOutgoingMessage msg = netClient.CreateMessage();
            msg.WritePadBits();
            msg.Write("COM:" + pName + ":" + message);
            netClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendLocalMessageToServer(string pName, string message)
        {
            NetOutgoingMessage msg = netClient.CreateMessage();
            msg.WritePadBits();
            msg.Write("MSG:" + pName + ":L:" + message);
            netClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendConnectionMessage(string pName)
        {
            NetOutgoingMessage msg = netClient.CreateMessage();
            msg.WritePadBits();
            msg.Write("CON:" + pName);
            netClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
