using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Combat
    {
        public enum DamageType
        {
            Blunt,
            Piercing,
            Fire,
            Water,
            Ice,
            Earth,
        }

        public static void PlayerAttack(string attacker, string receiver, Attack attack, bool isNPCReceiver)
        {
            Player playerA = Player.GlobalList[attacker.ToUpper()];

            //Dynamics are scary, careful here.
            dynamic playerB;

            if (isNPCReceiver)
            {
                playerB = NPC.GlobalList[receiver.ToUpper()];
            }
            else
            {
                playerB = Player.GlobalList[receiver.ToUpper()];
            }

            int damage = attack.Damage;

            string attackString = attack.AttackString.Replace("-a-", "`p" + playerA.Name + "`w");
            if (isNPCReceiver)
                attackString = attackString.Replace("-r-", "`n" + playerB.Name + "`w");
            else
                attackString = attackString.Replace("-r-", "`p" + playerB.Name + "`w");
            Chat.SendAnonymousMessageToTile(attackString, playerA);

            if (playerB.DodgeChance > Dice.D20(attack.HitChance))
            {
                string dodgeString = attack.DodgeString.Replace("-a-", "`p" + playerA.Name + "`w");
                if (isNPCReceiver)
                    dodgeString = dodgeString.Replace("-r-", "`n" + playerB.Name + "`w");
                else
                    dodgeString = dodgeString.Replace("-r-", "`p" + playerB.Name + "`w");
                Chat.SendAnonymousMessageToTile(dodgeString, playerA);
                return; //Dodged Return;
            }

            if(playerB.DamageTypeResistances.Contains(attack.DamageType))
                damage -= Dice.D20();
            damage -= playerB.GlobalDamageResistance;

            //Announce hitstring.
            string hitString = attack.HitString.Replace("-a-", "`p" + playerA.Name + "`w");
            if (isNPCReceiver)
                hitString = hitString.Replace("-r-", "`n" + playerB.Name + "`w");
            else
                hitString = hitString.Replace("-r-", "`p" + playerB.Name + "`w");
            hitString = hitString.Replace("-d-", "`y" + attack.ParticleString + damage.ToString() + "`w");
            Chat.SendAnonymousMessageToTile(hitString, playerA);
            //Deal damage.
            playerB.Health -= damage;

            if (isNPCReceiver)
                AttackPlayer(playerA.Name, playerB.Name);

        }

        public static void AttackNPC(string pName, string npcName, Attack attack)
        {
            Player playerA = Player.GlobalList[pName.ToUpper()];
            Player playerB = Player.GlobalList[npcName.ToUpper()];

            int damage = attack.Damage;

            string attackString = attack.AttackString.Replace("-a-", "`p" + playerA.Name + "`w");
            attackString = attackString.Replace("-r-", "`p" + playerB.Name + "`w");
            Chat.SendAnonymousMessageToTile(attackString, playerA);

            if (playerB.DodgeChance > Dice.D20(attack.HitChance))
            {
                string dodgeString = attack.DodgeString.Replace("-a-", "`p" + playerA.Name + "`w");
                dodgeString = dodgeString.Replace("-r-", "`p" + playerB.Name + "`w");
                Chat.SendAnonymousMessageToTile(dodgeString, playerA);
                return; //Dodged Return;
            }

            if (playerB.DamageTypeResistances.Contains(attack.DamageType))
                damage -= Dice.D20();
            damage -= playerB.GlobalDamageResistance;

            //Announce hitstring.
            string hitString = attack.HitString.Replace("-a-", "`p" + playerA.Name + "`w");
            hitString = hitString.Replace("-r-", "`p" + playerB.Name + "`w");
            Chat.SendAnonymousMessageToTile(hitString, playerA);
            //Deal damage.
            playerB.Health -= damage;

            AttackPlayer(playerA.Name, playerB.Name);
        }

        public static void AttackPlayer(string pName, string npcName)
        {
            Player ply = Player.GlobalList[pName.ToUpper()];
            NPC npc = NPC.GlobalList[npcName.ToUpper()];

            if (npc.Attacks.Count == 0)
                return;
            Attack attack = npc.Attacks[Dice.Random.Next(0,npc.Attacks.Count-1)];
            int damage = attack.Damage;

            //Attack message
            Chat.SendAnonymousMessageToTile(attack.ParticleString + npc.Name + attack.AttackString + ply.Name + ".", ply);

            //Check if it will miss.
            if(ply.DodgeChance > Dice.D20(attack.HitChance))
            {
                Chat.SendAnonymousMessageToTile("`p"+ply.Name + "`w swiftly dodges `n" + npc.Name + "'s`w attack.", ply);
                return;
            }

            //Check resistances;
            if (ply.DamageTypeResistances.Contains(attack.DamageType))
            {
                damage -= Dice.D20();
            }

            //Reduce damage by normal resistances.
            damage -= ply.GlobalDamageResistance;

            ply.Health -= damage;
            Chat.SendAnonymousMessageToTile("`p"+ply.Name + "`w is hit by `n" + npc.Name + "'s`w attack for `y" + attack.ParticleString + damage + "`w damage.", ply);
        }


        public class Attack
        {
            public int Damage = 20;
            public int HitChance = 2;
            public DamageType DamageType = DamageType.Blunt;
            public string ParticleString = "";
            public string AttackString = "-a- throws an enraged rock at -r-";
            public string DodgeString = "-r- swiftly dodges -a-'s attack ";
            public string HitString = "-r- is hit by -a-'s attack for -d- damage.";

            public Attack()
            {

            }
        }
    }
}
