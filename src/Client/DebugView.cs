using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Client
{
    class DebugView
    {
        private double currentTime = DateTime.Now.Millisecond;
        private double currentFPSTime = DateTime.Now.Second;
        private int frameCount = 0;
        private Font debugFont = new Font(@"SourceSansPro-Regular.otf");
        private Text frameText;
        private Text fpsText;

        public DebugView()
        {
            frameText = new Text("Frametime", debugFont);
            fpsText = new Text("FPS", debugFont);
            frameText.Position = new Vector2f(Program.Window.Size.X - 45, 0);
            frameText.CharacterSize = 14;
            frameText.Style = Text.Styles.Bold;
            fpsText.Position = new Vector2f(Program.Window.Size.X - 45, 20);
            fpsText.CharacterSize = 14;
            fpsText.Style = Text.Styles.Bold;

            Program.OnUpdate += Update;
            Program.OnDraw += Draw;
        }

        public void Update()
        {
            double newTime = DateTime.Now.Millisecond;
            float frameTime = (float)(newTime - currentTime);
            currentTime = newTime;
            if (frameTime > -500) //Check because DateTime resets.
                frameText.DisplayedString = frameTime.ToString() + "ms";

            double newFPSTime = DateTime.Now.Second;
            if (currentFPSTime != newFPSTime)
            {
                currentFPSTime = newFPSTime;
                fpsText.DisplayedString = frameCount.ToString() + "fps";
                frameCount = 0;
            }
            frameCount++;
        }

        public void Draw()
        {
            frameText.Color = Color.Black;
            frameText.Position = new Vector2f(frameText.Position.X + 1, frameText.Position.Y + 1);
            frameText.Draw(Program.Window, RenderStates.Default);
            frameText.Color = Color.White;
            frameText.Position = new Vector2f(frameText.Position.X - 1, frameText.Position.Y - 1);
            frameText.Draw(Program.Window, RenderStates.Default);
            fpsText.Color = Color.Black;
            fpsText.Position = new Vector2f(fpsText.Position.X + 1, fpsText.Position.Y + 1);
            fpsText.Draw(Program.Window, RenderStates.Default);
            fpsText.Color = Color.White;
            fpsText.Position = new Vector2f(fpsText.Position.X - 1, fpsText.Position.Y - 1);
            fpsText.Draw(Program.Window, RenderStates.Default);
        }
    }
}
