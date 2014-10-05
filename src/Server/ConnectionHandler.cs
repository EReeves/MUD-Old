using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Server
{
    //Handles new connections.
    class ConnectionHandler
    {
        public static void AddPlayer(string[] str, NetConnection con)
        {
             var ply = new Player(str[1], con);
             //Debug tile assignment.
             //Server.Tiles[new Vec2(1, 1)].PlayerEnter(ply.Name.ToUpper());
        }   

    }
}
