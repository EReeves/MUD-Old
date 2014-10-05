using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Client.GUI
{
    class Button : Sprite
    {
        public Text ButtonText = new Text();
        public bool Active = true;
        public bool Clicked = false;
        public event Program.ClientDelegate OnClick, OnFirstClick;
        private bool ClickOnNextFrame = false;
        private bool frame = false;

        public Button(Texture buttonTexture, string text)
        {
            Texture = buttonTexture;
            ButtonText.Font = Program.CConsole.Font;
            ButtonText.Color = Color.Black;
            ButtonText.DisplayedString = text;
            ButtonText.CharacterSize = 22;
            Program.OnUpdate += Update;
            Program.OnDraw += Draw;
            Program.Window.MouseButtonPressed += Window_MouseButtonPressed;
            Program.Window.MouseButtonReleased += Window_MouseButtonReleased;
        }

        void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (Active && e.Button == Mouse.Button.Left && MouseOver())
            {
                Color = Color.White;
            }
        }

        void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (Active && e.Button == Mouse.Button.Left && MouseOver())
            {
                Color = new Color(100, 100, 100);
                ButtonText.DisplayedString = "Loading..";
                ClickOnNextFrame = true;
            }
        }

        private void Update()
        {
            ButtonText.Position = Position + new Vector2f(Texture.Size.X/2 - ButtonText.GetLocalBounds().Width/2, 0);

            if(frame)
            {
                if (!Clicked)
                    if (OnFirstClick != null)
                        OnFirstClick();
                Clicked = true;
                if (OnClick != null)
                    OnClick.Invoke();
                frame = false;
            }

            if (ClickOnNextFrame)
                frame = true;
        }

        private void Draw()
        {
            if (Active)
            {
                Draw(Program.Window, RenderStates.Default);
                ButtonText.Draw(Program.Window, RenderStates.Default);
            }
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
