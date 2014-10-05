using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Spells
{
    class SpellInstancer
    {
        public static List<Spell> Spells = new List<Spell>();

        public SpellInstancer()
        {
            Spell HADOUKEN = new Spell("HADOUKEN!")
            {
                AttackString = "-a- sends a Hadouken towards -r-",
                Damage = 1000,
                HitChance = 10,
                particleCastString = "~t"
            };
            HADOUKEN.RegisterCheck();
            Spells.Add(HADOUKEN);
        }

        public static string[] SpellNames()
        {
            List<string> strings = new List<string>();
            foreach (Spell s in Spells)
            {
                strings.Add(s.CastString);
            }
            return strings.ToArray();
        }

        public static string GetParticleCastString(string caststring)
        {
            foreach (Spell s in Spells)
            {
                if (s.CastString == caststring.ToUpper())
                {
                    return s.particleCastString;
                }
            }
            return "";
        }
    }
}
