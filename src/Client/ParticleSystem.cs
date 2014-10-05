using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Client
{
    class ParticleSystem
    {
        public static ObjectPool<Particle> ParticlePool = new ObjectPool<Particle>(100);
        public float Speed = 0.1f;
        public float speedVariance = 20;
        public int lifetimeVariance = 5;
        public float directionVariance = 0.1f;
        public float speedAccumulator = 0.1f;
        public Random random = new Random(System.DateTime.Now.Millisecond);
        public Vector2f Position;

        public ParticleSystem(Vector2f position, Vector2f direction, int amount, int lifetime, float speed, Texture texture)
        {
            Position = position;
            

            for (int i = 0; i < amount; i++)
            {
                Particle par = ParticlePool.GetObject();
                par.Lifetime = lifetime + random.Next(-lifetimeVariance, lifetimeVariance);
                par.Texture = texture;
                par.Position = position;
                par.Direction = direction + new Vector2f(randomFloat(-directionVariance, directionVariance), randomFloat(-directionVariance, directionVariance));
                par.speedAccumulator = randomFloat(0,speedAccumulator);
                par.Speed = speed + randomFloat(-speedVariance,speedVariance);;
            }

        }

        float randomFloat(float a, float b)
        {
            return a + (float)random.NextDouble() * (b - a);
        }

        public class Particle : Sprite, IPoolable
        {
            public Vector2f Direction = new Vector2f(0, 1);
            public float Speed = 0.1f;
            public float speedAccumulator = 0.01f;
            public float Lifetime = 10;
            public bool Active = false;
            public bool Free { get; set; }
            public int count = 0;
            
            private void Update()
            {
                if (count > Lifetime)
                {
                    Pool();
                    return;
                }
                Position += Direction * Speed;
                Speed += speedAccumulator;
                count++;
            }

            private void Draw()
            {
                Position += new Vector2f(0,Program.CConsole.offset.Y);
                Position += new Vector2f(0,Program.CConsole.mwoffset.Y);
                Draw(Program.Window, RenderStates.Default);
                Position -= new Vector2f(0,Program.CConsole.offset.Y);
                Position -= new Vector2f(0,Program.CConsole.mwoffset.Y);
            }

            public void Pool()
            {
                Active = false;
                count = 0;
                Program.OnUpdate -= Update;
                Program.OnDraw -= Draw;
                Free = true;
            }

            public void UnPool()
            {
                Active = true;
                Program.OnUpdate += Update;
                Program.OnDraw += Draw;
                Free = false;
            }
        }
    }
}
