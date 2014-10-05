using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Client.GUI.ParticleEffects
{
    class ParticleSwitch
    {
        public static void SortParticleString(string particleString, Vector2f position)
        {
            switch (particleString)
            {
                case "t":
                    Color[,] c = new Color[1,1];
                    c[0, 0] = Color.Yellow;
                    Image i = new Image(c);
                    ParticleSystem par = new ParticleSystem(position, new Vector2f(0, 0), 200, 10, 35.4f, new Texture(i));
                    Program.ClientDelegate del = null;
                    int count = 0;
                    del = () =>
                    {
                        Program.Shake = true;
                        count++;
                        if (count > 6)
                        {
                            Program.Shake = false;
                            Program.OnUpdate -= del;
                        }
                    };
                    Program.OnUpdate += del;
                    break;
            }
                
        }
    }
}
