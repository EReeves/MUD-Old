using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Server
{
    class IncomingMessageHandler //Ugly class.
    {
        public IncomingMessageHandler()
        {
        }

        public void Start()
        {
            NetIncomingMessage msg;
            //Register message receiving loop in Update.
            Server.OnUpdate += () =>
            {
                msg = Server.NetServer.ReadMessage();
                if (msg != null)
                {
                    string[] splitStr = msg.ReadString().Split(':');
                    if (splitStr[0].Length > 0)
                    {
                        if (msg.MessageType == NetIncomingMessageType.Data && splitStr[0] != "CON" &&
                            !Player.GlobalList.ContainsKey(splitStr[1].ToUpper()))
                        { Console.WriteLine(splitStr[0] + ":ERROR:Player does not exist."); ConnectionHandler.AddPlayer(splitStr, msg.SenderConnection);}

                        switch (splitStr[0])
                        {
                            case "CON":
                                ConnectionHandler.AddPlayer(splitStr, msg.SenderConnection);
                                break;
                            case "MSG":
                                Chat.SortMessage(splitStr);
                                break;
                            case "COM":
                                Command.Sort(splitStr);
                                break;
                            default:
                                Console.WriteLine(splitStr[0]);
                                break;
                        }
                    }
                    Server.NetServer.Recycle(msg);
                }

                    
            };
        }
    }
}
