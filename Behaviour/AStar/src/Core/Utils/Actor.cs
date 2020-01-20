using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AStar.src.Core.BehaviourCore;


namespace AStar.src.Core.Utils
{
    class Actor
    {
        public static List<Actor> Actors = new List<Actor>();
        public List<Behaviour> BehaviourList = new List<Behaviour>();

        static Random random = new Random();

        public Color Color;
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public Vector2 Origin;

        public static Vector2 GetRandomPosition(int rangeX,int rangeY) {
            return new Vector2( random.Next(rangeX), random.Next(rangeY));
        }

        public static Vector2 GetRandomDirection()
        {
            double rotation = random.NextDouble()* MathHelper.TwoPi;
            return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        public Actor(Texture2D texture,Color color) {
            Actors.Add(this);
            Color = color;
            Texture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);

        }

        public void Update() {

            foreach (var behaviour in BehaviourList) {
                behaviour.Update(this);
            }
            if (Direction.Length() > 0.0f) {
                Direction.Normalize();
                }

            Position += Direction * Speed;
        }

        public void Draw(SpriteBatch spriteBatch) {
            float rotation = (float)Math.Atan2(Direction.Y, Direction.X)+MathHelper.PiOver2;

            spriteBatch.Draw(Texture, Position + new Vector2(10, 10), null, Color.Black*0.3f, rotation, Origin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(Texture, Position, null, Color, rotation,Origin,1.0f,SpriteEffects.None,0.0f);
        }
    }
}
