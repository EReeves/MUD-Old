using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using System.Text.RegularExpressions;

namespace Client.GUI
{
    //This class turned into a nightmare, steer clear.
    class Console
    {
        public bool Active = true;
        private ObjectPool<PooledText> textPool = new ObjectPool<PooledText>(20);
        private List<PooledText> textList = new List<PooledText>();
        public List<string> PureText = new List<string>();
        public Font Font;
        public Vector2f offset = new Vector2f(3, 0);
        public Vector2f mwoffset = new Vector2f(0, 0);
        private int lineCount;
        private int maxCharPerLine;
        public Text inputText;
        private List<string> inputHistory = new List<string>();
        private int inputHistoryPosition = 0;
        private int previousInputHistoryPosition = 0;

        public Console()
        {
            Font = new Font("FiraSansOT-Regular.otf");//"SourceSansPro-Regular.otf");
            maxCharPerLine = GetEstimatedWindowCharLength();
            inputText = new Text("", Font, 14);
            inputText.Position = new Vector2f(1, Program.Window.Size.Y - inputText.GetLocalBounds().Height);
            Program.OnUpdate += () => { if (Active) Update(); };
            Program.OnDraw += () => { if (Active) Draw(); };
            Program.Window.TextEntered += Window_TextEntered;
            Program.Window.MouseWheelMoved += Window_MouseWheelMoved;
            Program.Window.KeyPressed += Window_KeyPressed;
        }

        void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (!TextField.FieldActive && inputHistory.Count > 0)
            {
                if (e.Code == Keyboard.Key.Up)
                {
                    previousInputHistoryPosition = inputHistoryPosition;
                    if (inputHistoryPosition == 0)
                        inputHistoryPosition = inputHistory.Count;

                    inputHistoryPosition -= 1;
                    inputText.DisplayedString = inputHistory[inputHistoryPosition];
                }
                
                if (e.Code == Keyboard.Key.Down)
                {
                    previousInputHistoryPosition = inputHistoryPosition;

                    if (inputHistoryPosition+1 >= inputHistory.Count)
                        inputHistoryPosition = -1;

                    inputHistoryPosition += 1;
                    inputText.DisplayedString = inputHistory[inputHistoryPosition];
                }
            }

            //PASTE
            if(e.Control && Keyboard.IsKeyPressed(Keyboard.Key.V))
            {
                string s = System.Windows.Forms.Clipboard.GetText(System.Windows.Forms.TextDataFormat.UnicodeText);
                inputText.DisplayedString += s;
            }
        }

        void Window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            //Text Scrolling.
            if (e.Delta < 0 && mwoffset.Y+offset.Y > offset.Y)
                mwoffset -= new Vector2f(0,14);
            else if (e.Delta > 0 && offset.Y + mwoffset.Y < 0)
                mwoffset += new Vector2f(0, 14);
        }

        public void ResetText()
        {
            maxCharPerLine = GetEstimatedWindowCharLength();
           string[] pt = PureText.ToArray();
            textList.Clear();
            lineCount = 0;
            mwoffset = new Vector2f(0, 0);
            offset = new Vector2f(3, 0);
            foreach (string tl in pt)
            {
                //tl.Text = tl.PureText; //Force it to recalculate all the chunks.
                //ow
                WriteLine(tl);
            }

            while (textList.Count > 0 && 32 + offset.Y + FindLastText().Position.Y > Program.Window.Size.Y)
            {
                offset -= new Vector2f(0, 8f);
            }
            
        }

        void Window_TextEntered(object sender, SFML.Window.TextEventArgs e)
        {
            if (!TextField.FieldActive)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Back))
                {
                    if (inputText.DisplayedString.Length > 0)
                        inputText.DisplayedString = inputText.DisplayedString.Substring(0, inputText.DisplayedString.Length - 1);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                {
                    SortInput.SortInputString(inputText.DisplayedString);

                    //Add to input history.
                    if (!string.IsNullOrWhiteSpace(inputText.DisplayedString) && !inputHistory.Contains(inputText.DisplayedString))
                    {
                        if (inputHistory.Count > 6)
                        {
                            //Small List so this is fine.
                            inputHistory.RemoveAt(0);
                        }
                        inputHistory.Add(inputText.DisplayedString);
                        previousInputHistoryPosition = inputHistory.Count ;

                    }

                    inputText.DisplayedString = "";
                    if(previousInputHistoryPosition >= 0)
                    inputHistoryPosition = previousInputHistoryPosition;
                }
                else
                    inputText.DisplayedString += e.Unicode;

                inputText.DisplayedString.Replace("\b", "");
            }
        }

        private int GetEstimatedWindowCharLength()
        {
            //TODO: Fix, it isn't font specific, not urgent.
            Text t = new Text("g", Font);
            t.CharacterSize = 14;
            while (true)
            {
                t.DisplayedString += "L";//L because it is wide.
                //This is just a rough estimation so it doesn't have to work so hard to figure out the position of the wrap.
                if (t.GetLocalBounds().Width + 14 > Program.Window.Size.X)
                {
                    break;
                }
            }
            return t.DisplayedString.Length + 25;
        }

        public void WriteLine(string str)
        {
            bool skipLineCount = false;
            PooledText tex = textPool.GetObject();
            tex.Position = new Vector2f(0, (14 * lineCount));
            tex.ParticleY = 0; ;
            tex.Font = Font;
            tex.Text = str;
            tex.CharacterSize = 14;
            
            

            if (str.Contains("\n")) //Split any new lines and send back through this method.
            {
                string[] newlineSplit = Regex.Split(str, "\n");
                string colourAddition = "";
                bool cASkipNextWrite = false;
                foreach (string ns in newlineSplit)
                {
                    if (string.IsNullOrEmpty(ns))
                        continue;

                    if (ns.Contains("`")) //If it was using a colour set it on the new line too.
                        for (int i = ns.Length-1; i > 0; i--)
                        {
                            if (ns[i] == '`')
                            {
                                WriteLine(ns);
                                colourAddition = "`" + ns[i + 1];
                                cASkipNextWrite = true;
                                break;
                            }
                        }

                    if (!cASkipNextWrite)
                    {
                        WriteLine(colourAddition + ns);
                        colourAddition = "";
                    }
                    cASkipNextWrite = false;
                }
                return;
            }
           
            string wwColourAddition = "";

            if (str.Length > maxCharPerLine) //Word Wrap.
            for (int i = maxCharPerLine; i > 0; i--)
            {
                if (str[i] == ' ')
                {
                    tex.Text = str.Substring(0, i);
                    if (tex.GetLocalWidth() > Program.Window.Size.X)
                        continue;
                    skipLineCount = true;
                    lineCount++;
                    //Check for colours and pass on to next string.
                    if (tex.PureText.Contains('`'))
                        for (int o = tex.PureText.Length - 1; o > 0; o--)
                        {
                            if (tex.PureText[o] == '`')
                            {
                                wwColourAddition = "`" + tex.PureText[o + 1];
                                break;
                            }
                        }

                    WriteLine(wwColourAddition + str.Substring(i+1,str.Length-i-1));
                    wwColourAddition = "";
                    break;
                }
            }
  

            textList.Add(tex);
            //Scroll text.
            int chunkCount = tex.TextChunks.Count();
            Text[] chunks = new Text[tex.TextChunks.Length];
            for (int u = 0; u < chunks.Length; u++)
            {
                chunks[u] = new Text(tex.TextChunks[u]);
                tex.TextChunks[u].DisplayedString = "";
            }
            int chunkPosition = 0;
            int internalChunkPosition = 0;
            Program.ClientDelegate delDelegate = null;
            delDelegate = () =>
            {
                for (int p = 0; p < 2; p++)
                {
                    if (chunks[chunkPosition].DisplayedString.Length > internalChunkPosition)
                    {
                        internalChunkPosition += 4;
                        if (internalChunkPosition > chunks[chunkPosition].DisplayedString.Length)
                            internalChunkPosition -= 3;
                        tex.TextChunks[chunkPosition].DisplayedString = chunks[chunkPosition].DisplayedString.Substring(0, internalChunkPosition);
                    }
                    else
                    {
                        internalChunkPosition = 0;
                        chunkPosition++;
                        if (chunkPosition >= chunks.Length)
                        {
                            Program.OnUpdate -= delDelegate;
                            break;
                        }
                    }
                }
            };
            Program.OnUpdate += delDelegate;


            if(!skipLineCount)
                lineCount++;
        }

        private void Update()
        {
            //Scroll Text;
            if (textList.Count > 0 && 32 + offset.Y + FindLastText().Position.Y> Program.Window.Size.Y)
            {
                offset -= new Vector2f(0, 8f);
            }

            //Cull old texts.
            if (textList.Count > 200)
            {
                textList.RemoveAt(0); //:S
            }

            if (PureText.Count > textList.Count)
            {
                PureText.RemoveAt(0);
            }
                
            //Removes a rogue character, just leave this in. Don't question it.
            inputText.DisplayedString = inputText.DisplayedString.Replace("","");

        }

        private RichTextLine FindLastText()
        {//This is pretty bad but I would need to rewrite a TON to do it better.
            RichTextLine max = null; ;
            
            foreach (RichTextLine t in textList)
            {
                if (max == null || t.Position.Y > max.Position.Y)
                {
                    max = t;
                }
            }
            return max;
        }


        private void Draw()
        {
            foreach (RichTextLine t in textList)
            {
                t.Position += offset;
                t.Position += mwoffset;
                t.Draw();
                t.Position -= offset;
                t.Position -= mwoffset;
            }
            inputText.Position -= new Vector2f(0, inputText.GetLocalBounds().Height + 7);
            inputText.Draw(Program.Window, RenderStates.Default);
            inputText.Position += new Vector2f(0, inputText.GetLocalBounds().Height + 7);
        }
    }

    class PooledText : RichTextLine, IPoolable
    {
        public bool Free { get; set; }

        public void Pool()
        {

        }

        public void UnPool()
        {

        }
    }
}
