using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;

namespace Client
{
    class ActionQueue
    {
        private int currentTime;
        private Queue<QueueAction> actionList = new Queue<QueueAction>();
        private QueueAction previousAction = null;

        public ActionQueue()
        {
            currentTime = Program.ElapsedGameTime;
            Program.OnUpdate += Update;
        }

        public void Update()
        {
            int newTime = Program.ElapsedGameTime;
            if (actionList.Count >= 1)
            {
                if (newTime > currentTime + actionList.Peek().Interval)
                {
                    if (previousAction == null || previousAction.Task.IsCompleted)
                    {
                        currentTime = newTime;
                        actionList.Dequeue().Invoke();
                    }
                }
            }
        }

        public void Add(int interval, Action action)
        {
            //Convert interval from frames to realtime.
            actionList.Enqueue(new QueueAction(interval, action));
        }

        public static void AddRelative(int interval, Action action)
        {
            //Self contained, will unhook itself from OnUpdate via delegate wizardry.
            int elapseTime = interval + Program.ElapsedGameTime;
            Program.ClientDelegate delAction = null;
            delAction = delegate()
            {
                int newTime = Program.ElapsedGameTime;
                if (newTime > elapseTime)
                {
                    Task.Factory.StartNew(action);
                    Program.OnUpdate -= delAction;
                }
            };
            Program.OnUpdate += delAction;
        }

        /// <param name="repeatInterval">A lazy modulus check, so 2 = every second frame, 10 = every tenth frame etc.</param>       
        /// <param name="repeatAmount">0 = infinite, use sparingly.</param>
        public static void AddRelativeRepeat(int startInterval, int repeatInterval, int repeatAmount, Action action)
        {
            bool active = false;
            int count = 0;
            int elapseTime = startInterval + Program.ElapsedGameTime;
            Program.ClientDelegate delAction = null;
            delAction = delegate()
            {
                int newTime = Program.ElapsedGameTime;
                if (newTime > elapseTime)
                    active = true;

                if (active && Program.ElapsedGameTime % repeatInterval == 0)
                {
                    Task.Factory.StartNew(action);
                    count++;
                    if (count >= repeatAmount && repeatAmount != 0)
                        Program.OnUpdate -= delAction;
                }
            };
            Program.OnUpdate += delAction;
        }

    }

    class QueueAction
    {
        public int Interval { get; set; }
        private Action Action;
        public Task Task;

        public QueueAction(int interval, Action action)
        {
            Action = action;
            Interval = interval;
        }

        public void Invoke()
        {
            Task = Task.Factory.StartNew(Action);
        }
    }

}