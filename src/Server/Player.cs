using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Server
{
    class Player
    {
        public static Dictionary<string, Player> GlobalList = new Dictionary<string, Player>();

        public Inventory Inventory;
        public Tile CurrentTile;
        public NetConnection Connection;
        public string Name = "NoName";
        public bool InConversation = false;
        public Conversation CurrentConversation = null;

        //Stats
        public int Health;
        public int MaxHealth = 100;
        public List<Combat.DamageType> DamageTypeResistances = new List<Combat.DamageType>();
        public int DodgeChance = 20;
        public int GlobalDamageResistance = 5;
        //Combat

        public Player(string name, NetConnection con)
        {
            Name = name;
            Connection = con;
            Inventory = new Inventory(this);
            if(!GlobalList.ContainsKey(name.ToUpper()))
                GlobalList.Add(name.ToUpper(), this);
            Server.OnUpdate += new Server.ServerDelegate(Update);
            PlayerSpawn();
        }

        public void Update()
        {
            if (Health <= 0)
                PlayerDeath();
        }

        private void PlayerDeath()
        {
            Chat.SendAnonymousMessageToTile(Name + " has died.", this);
            PlayerSpawn();
        }

        private void PlayerSpawn()
        {
            Health = MaxHealth;
            Tile.DefaultTile.PlayerEnter(Name);
        }

    }
}
