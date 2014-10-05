using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Client.GUI
{
    class TextField : Sprite
    {
        public static List<TextField> GlobalList = new List<TextField>();
        public static bool FieldActive = false;

        private string text = "";
        public string Text { get { return text; } set { text = value; } }
        private Text sText = new Text();
        public bool Active = true;
        public bool TextActive = false;

        private Vector2f position = new Vector2f();

        public TextField(Texture fieldTexture)
        {
            Texture = fieldTexture;
            Color = new Color(150, 150, 150);
            sText.Font = Program.CConsole.Font;
            sText.Color = Color.Black;
            sText.CharacterSize = 14;
            Program.OnUpdate += () => { if(Active)Update(); };
            Program.OnUpdate += ActiveCheck;
            Program.OnDraw += () => { if (Active)Draw(); };      
            Program.Window.MouseButtonPressed += Window_MouseButtonPressed;
            Program.Window.TextEntered += Window_TextEntered;

            GlobalList.Add(this);
        }

        private static void ActiveCheck()
        {
            bool active = false;
            foreach (TextField t in GlobalList)
            {
                if (t.TextActive == true)
                { active = true; break; }
            }
            FieldActive = active;
        }

        void Window_TextEntered(object sender, TextEventArgs e)
        {
            if (TextActive && Keyboard.IsKeyPressed(Keyboard.Key.Back))
            {
                if(text.Length > 0)
                    text = text.Substring(0,text.Length-1);
            }
            else
            if (Active && TextActive)
            {
                text += e.Unicode;
            }
        }

        void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (Active && e.Button == Mouse.Button.Left)
            {
                bool skip = false;
                if (!TextActive && MouseOver())
                { TextActive = true; Color = Color.White; skip = true; }

                if (!skip && TextActive && !MouseOver())
                { TextActive = false; Color = new Color(150, 150, 150);}
            }
        }

        private void Update()
        {
            position = Position;
            sText.Position = Position + new Vector2f(5, 5);
            sText.DisplayedString = text;

            
        }

        private void Draw()
        {
            Draw(Program.Window, RenderStates.Default);
            sText.Draw(Program.Window, RenderStates.Default);
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
