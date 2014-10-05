using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace TestClient
{
    class Program
    {

        static void Main(string[] args)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("SF");
            config.Port = 14041;

            NetClient client = new NetClient(config);
            client.Start();

            client.Connect("127.0.0.1", 14040);

            while (true)
            {
                Console.WriteLine(": ");
                string str = Console.ReadLine();

                NetOutgoingMessage msg = client.CreateMessage();
                msg.WritePadBits();
                msg.Write(str);
                client.SendMessage(msg, NetDeliveryMethod.Unreliable);
            }
            
        }
    }
}
