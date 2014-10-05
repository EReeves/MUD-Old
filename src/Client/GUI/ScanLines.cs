using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Client.GUI
{
    class ScanLines
    {
        public Sprite lineSprite = new Sprite();

        public ScanLines()
        {
            Color[,] pixel = new Color[1,1];
            pixel[0,0] = new Color(0,0,0,70);
            Image img = new Image(pixel);
            lineSprite.Texture = new Texture(img, new IntRect(0, 0, (int)Program.Window.Size.X, 1));
            lineSprite.Position = new Vector2f(1, 1);
            lineSprite.TextureRect = new IntRect(0, 0,  (int)Program.Window.Size.X, 1);
            Program.OnDraw += Draw;
        }

        public void Reset()
        {
            lineSprite.TextureRect = new IntRect(0, 0, (int)Program.Window.Size.X, 1);
        }

        private void Draw()
        {
            for (int i = 0; i < Program.Window.Size.Y; i += 2)
            {
                lineSprite.Position = new Vector2f(1, i);
                lineSprite.Draw(Program.Window, RenderStates.Default);
            }
        }

    }
}
