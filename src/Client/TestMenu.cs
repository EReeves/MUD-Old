using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.GUI;
using SFML.Window;
using SFML.Graphics;
using System.IO;

namespace Client
{
    class TestMenu
    {
        private bool Active = true;

        public TextField IPField;
        public TextField NameField;
        public Button StartButton;

        Text iptex;
        Text nametex;

        public TestMenu()
        {
            Texture t = new Texture(@"Data/Sprites/TextField.png");
            int x = (int)Program.Window.Size.X / 2 - (int)(t.Size.X/2);
            IPField = new TextField(t);
            IPField.Position = new Vector2f(x, 200);
            IPField.Text = File.ReadAllText("Data/ip");
            iptex = new Text("IP Address", Program.CConsole.Font);
            iptex.Position = new Vector2f(x, 180);
            iptex.CharacterSize = 16;
            NameField = new TextField(t);
            NameField.Position = new Vector2f(x, 250);
            nametex = new Text("Name", Program.CConsole.Font);
            nametex.Position = new Vector2f(x, 230);
            nametex.CharacterSize = 16;
            StartButton = new Button(t, "Start");
            StartButton.Position = new Vector2f(x, 300);
            StartButton.OnFirstClick += () =>
            {
                StartButton.ButtonText.DisplayedString = "Loading..";
            };
            StartButton.OnFirstClick += Start;
            Program.OnDraw += Draw;

            Program.Window.KeyPressed += Window_KeyPressed;
        }

        public void ResetPositions()
        {
            int x = (int)Program.Window.Size.X / 2 - (int)(IPField.Texture.Size.X / 2);
            IPField.Position = new Vector2f(x, 200);
            iptex.Position = new Vector2f(x, 180);
            NameField.Position = new Vector2f(x, 250);
            nametex.Position = new Vector2f(x, 230);
            StartButton.Position = new Vector2f(x, 300);
        }

        void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (Active && e.Code == Keyboard.Key.Return)
            {
               // Active = false;
                //Start();
            }
        }

        private void Start()
        {
            Program.PlayerName = NameField.Text;
            Program.CNetwork = new Network(IPField.Text);
          //  DebugView dv = new DebugView();
            IPField.Active = false;
            NameField.Active = false;
            StartButton.Active = false;
            iptex.Color = Color.Transparent;
            nametex.Color = Color.Transparent;
        }

        private void Draw()
        {
            iptex.Draw(Program.Window, RenderStates.Default);
            nametex.Draw(Program.Window, RenderStates.Default);
        }
    }
}
