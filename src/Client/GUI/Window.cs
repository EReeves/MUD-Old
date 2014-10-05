using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Client.GUI
{
    class Window : Sprite
    {
        public bool Active = true;
        private bool dragging = false;
        private Vector2f dragPos = new Vector2f(0, 0);
        public Vector2f mousePosOnDragStart = new Vector2f(0,0);
        public List<Sprite> Controls = new List<Sprite>();

        public Window()
        {   
            Texture = new SFML.Graphics.Texture("Data/Sprites/Window.png");
            Program.OnUpdate += () => { if (Active) Update(); };
            Program.OnDraw += () => { if (Active) Draw(); };
            Program.Window.MouseButtonPressed += Window_MouseButtonPressed;
            Program.Window.MouseButtonReleased += Window_MouseButtonReleased;
        }

        void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Right)
            {
                dragging = false;
            }
        }

        void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Right)
            {
                dragging = true;
                dragPos = new Vector2f(e.X, e.Y);
                mousePosOnDragStart = Position - dragPos;
            }
        }

        public byte Alpha = 255;
        private void Update()
        {
            if (dragging)
                Position = mousePosOnDragStart + dragPos + new Vector2f(Mouse.GetPosition(Program.Window).X - dragPos.X, Mouse.GetPosition(Program.Window).Y - dragPos.Y);

            for (int o = 0; o < 2; o++)
            {
                if (!MouseOver())
                {
                    if (Alpha > 30)
                        Alpha -= 12;
                    Color = new Color(255, 255, 255, Alpha);
                    foreach (Sprite s in Controls)
                        s.Color = new Color(255, 255, 255, Alpha);
                }
                else
                {
                    if (Alpha < 255)
                        Alpha += 12;
                    if (Alpha < 200)
                    {
                        Color = new Color(255, 255, 255, Alpha);
                    }
                    foreach (Sprite s in Controls)
                        s.Color = new Color(255, 255, 255, Alpha);
                }
            }
        }

        private void Draw()
        {
            Draw(Program.Window, RenderStates.Default);
        }


        private bool MouseOver()
        {
            float xDist = System.Math.Abs(Position.X - Mouse.GetPosition(Program.Window).X + Texture.Size.X / 2);
            float yDist = System.Math.Abs(Position.Y - Mouse.GetPosition(Program.Window).Y + Texture.Size.Y / 2);
            float calculatedGapX = xDist - (Texture.Size.X / 2) - 0.5f;
            float calculatedGapY = yDist - (Texture.Size.Y / 2) - 0.5f;

            if (calculatedGapX < 0 && calculatedGapY < 0)
                return true;    //Rectangles intersect.
            else
                return false;
        }
    }
}
