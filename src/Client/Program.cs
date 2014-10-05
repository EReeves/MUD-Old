using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using System.IO;

namespace Client
{
    class Program
    {
        public delegate void ClientDelegate();
        public static event ClientDelegate OnUpdate, OnDraw;
        public static bool Shake = false;
        public static View PreShakeView;
        public static RenderWindow Window;
        public static Network CNetwork;
        public static GUI.Console CConsole;
        public static GUI.ScanLines sL;
        public static TestMenu TestMenu;
        public static string PlayerName = "UnNamed";
        public static int ElapsedGameTime = 0;
        public static GUI.Map Map;

        [STAThread]
        static void Main(string[] args)
        {
            ContextSettings contextSettings = new ContextSettings()
            {
                DepthBits = 32,
                AntialiasingLevel = 8
            };
            Window = new RenderWindow(new VideoMode(700, 500), "Client", Styles.Default , contextSettings);
            Window.SetFramerateLimit(61);
            Window.SetActive();
            Window.Closed += Window_Closed;
            Window.Resized += Window_Resized;

            sL = new GUI.ScanLines();
            Program.CConsole = new GUI.Console();
            TestMenu = new TestMenu();

            Map = new GUI.Map();
            /*
            PlayerName = LoadName();
            CNetwork = new Network();
            CConsole = new GUI.Console();
            DebugView dv = new DebugView();*/
            PreShakeView = Window.GetView();
            Random random = new Random(DateTime.Now.Millisecond);

            while (Window.IsOpen())
            {
                Window.DispatchEvents();
                Window.Clear(new Color(50, 50, 50));
                if(OnUpdate != null)
                    OnUpdate.Invoke();

                if (Shake)
                {
                    View view = Window.GetView();
                    view.Move(new Vector2f(random.Next(-2, 3), random.Next(-2, 2)));
                    Window.SetView(view);
                }
                else
                {
                    View newView = new View(new FloatRect(0, 0, Window.Size.X, Window.Size.Y));
                    Window.SetView(newView);
                }

                ElapsedGameTime++;
                if (OnDraw != null)
                    OnDraw.Invoke();
                Window.Display();
            }
        }

        static void Window_Resized(object sender, SizeEventArgs e)
        {
            View newView = new View(new FloatRect(0,0,e.Width,e.Height));
            Window.SetView(newView);
            Window.Size = new Vector2u(e.Width, e.Height);
            CConsole.ResetText();
            sL.Reset();
            Map.Reset();
            TestMenu.ResetPositions();
            CConsole.inputText.Position = new Vector2f(1, Window.Size.Y - CConsole.inputText.GetLocalBounds().Height);
            PreShakeView = Window.GetView();
        }

        private static string LoadName()
        {
            return File.ReadAllText("Data/name");
        }

        static void Window_Closed(object sender, EventArgs e)
        {
            //Aww.
            RenderWindow Window = (RenderWindow)sender;
            Window.Close();
        }

    }
}
