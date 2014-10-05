using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Client
{
    class RichTextLine
    {
        public Text[] TextChunks;
        public Font Font;
        public float ParticleY = 0;

        private Vector2f position;
        public Vector2f Position
        {
            get { return position; }
            set 
            {
                position = value;
                int width = 0;
                if (TextChunks != null)
                {
                    foreach (Text tc in TextChunks)
                    {
                        if (tc != null)
                        {
                            tc.Position = value + new Vector2f(width, 0);
                            width += (int)tc.GetLocalBounds().Width;
                        }
                    }
                }
            }
        }

        private uint characterSize = 14;
        public uint CharacterSize
        {
            get { return characterSize; }
            set
            {
                characterSize = value;
                if(TextChunks == null || TextChunks.Count() < 1)
                    return;
                foreach (Text tc in TextChunks)
                {
                    if (tc != null)
                    {
                        tc.CharacterSize = characterSize;
                    }
                }
                RecalculateChunkSpacing();
            }
        }

        public string PureText { get; private set; }
        private string text = "";
        public string Text
        {
            get { return text; }
            set
            {
                if (Font == null)
                    throw new Exception("No font set on RichTextLine.");
                PureText = ("`w" + value);
                text = "";
                string[] split = PureText.Split('`');
                TextChunks = new Text[split.Length - 1];
                int width = 0;
                ActionQueue aQ = new ActionQueue();
                for (int i = 1; i < split.Length; i++) //Offset by one because the first split will be empty.
                {
                    TextChunks[i - 1] = new Text("", Font);
                    TextChunks[i - 1].DisplayedString = split[i].Substring(1, split[i].Length - 1);
                    TextChunks[i - 1].Color = GetColor(split[i].First());
                    TextChunks[i - 1].Position = position + new Vector2f(width, 0);
                    TextChunks[i - 1].CharacterSize = characterSize;
                    width += (int)TextChunks[i - 1].GetLocalBounds().Width;
                    text += TextChunks[i - 1].DisplayedString;

                    if(split[i].Contains("~"))
                    {
                        string[] particleSplit = split[i].Split('~');
                        foreach (string s in particleSplit)
                        {
                            Vector2f pos = TextChunks[i - 1].Position + new Vector2f((TextChunks[i - 1].GetLocalBounds().Width / 2), TextChunks[i - 1].GetLocalBounds().Height/2 - ParticleY);
                            GUI.ParticleEffects.ParticleSwitch.SortParticleString(s.First().ToString(), pos);
                        }

                        for(int c=0;c<TextChunks[i - 1].DisplayedString.Count();c++)
                        {
                            if (TextChunks[i - 1].DisplayedString[c] == '~')
                            {
                                TextChunks[i - 1].DisplayedString = TextChunks[i - 1].DisplayedString.Remove(c, 2);  
                            }
                        }
                    }

                }

            }
        }

        public void RecalculateChunkSpacing()
        {
            int width = 0;
            foreach (Text tc in TextChunks)
            {
                if (tc != null)
                {
                    tc.Position = position + new Vector2f(width, 0);
                    width += (int)tc.GetLocalBounds().Width;
                }
            }
        }

        public int GetLocalWidth()
        {
            float width = 0;
            foreach (Text tc in TextChunks)
            {
                if (tc != null)
                {
                    width += tc.GetLocalBounds().Width;
                }
            }
            return (int)width;
        }

        public RichTextLine()
        {
        }

        public Color GetColor(char str)
        {
            switch (str)
            {
                case 'w':
                    return Color.White;
                case 'x':
                    return new Color(100,100,100); // DArk Grey
                case 'g':
                    return new Color(200, 200, 200); // Grey
                case 'b':
                    return new Color(80, 170, 255);//Light Blue
                case 'r':
                    return Color.Red;
                case 'y':
                    return new Color(250, 250, 125); //Yellow
                case 'o':
                    return new Color(255,178,51);//Orange
                case 'k':
                    return Color.Black;
                case 'n':
                    return new Color(160, 240, 160); //Green
                case 'p':
                    return new Color(190, 170, 255); // Purple

            }
            return Color.White;
        }

        public void Draw()
        {
            foreach (Text t in TextChunks)
            {
                Color original = t.Color;
                t.Color = Color.Black;
                t.Position += new Vector2f(0, 1);
                t.Draw(Program.Window, RenderStates.Default);
                t.Position -= new Vector2f(0, 1);
                t.Color = original;
                t.Draw(Program.Window, RenderStates.Default);
            }
        }
    }
}
