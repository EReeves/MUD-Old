using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Vec2
    {
        public int X;
        public int Y;
        public Vec2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2 operator *(Vec2 a, int b)
        {
            return new Vec2(a.X * b, a.Y * b);
        }

        public override int GetHashCode()
        {
            //Don't need this.
            return 1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Vec2 a = obj as Vec2;
            return a.X == this.X && a.Y == this.Y;
        }
    }
}
