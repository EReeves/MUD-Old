using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Client.GUI
{
    class Map : Window
    {
        Sprite mapSprite = new Sprite();
        Sprite dotSprite = new Sprite();
        public Vector2f mapSpriteOffset = new Vector2f(0, 0);
        private Vector2f mapSpriteOffsetSoft = new Vector2f(0, 0);
        
        public Map()
        {
            
            mapSprite.Texture = new Texture("Data/Sprites/Map.png");
            dotSprite.Texture = new SFML.Graphics.Texture("Data/Sprites/Dot.png");
            Position = new Vector2f(Program.Window.Size.X - Texture.Size.X, 0);
            Controls.Add(mapSprite);
            Controls.Add(dotSprite);
            Program.OnUpdate += MapSpritePositionUpdate;
            Program.OnDraw += DrawMapSprite;
        }

        public void Reset()
        {
            Position = new Vector2f(Program.Window.Size.X - Texture.Size.X, 0);
        }

        private void MapSpritePositionUpdate()
        {
            Vector2f direction = (mapSpriteOffset - mapSpriteOffsetSoft) / 10;               
            mapSpriteOffsetSoft += direction;
            if (Math.Abs(mapSpriteOffset.X - mapSpriteOffsetSoft.X) > 1 || Math.Abs(mapSpriteOffset.Y - mapSpriteOffsetSoft.Y) > 1)
                Program.Map.Alpha = 255;
            
            mapSprite.Position = Position + new Vector2f(Texture.Size.X/2, Texture.Size.Y/2) - mapSpriteOffsetSoft;
            dotSprite.Position = Position + new Vector2f(Texture.Size.X / 2, Texture.Size.Y / 2) - new Vector2f(dotSprite.Texture.Size.X / 2, dotSprite.Texture.Size.Y / 2);
        }

        private void DrawMapSprite()
        {
            if (mapSpriteOffset.X == 0 && mapSpriteOffset.Y == 0)
                return;
            mapSprite.Draw(Program.Window, RenderStates.Default);
            dotSprite.Draw(Program.Window,RenderStates.Default);
        }

    }
}
