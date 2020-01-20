using AStar.src.Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar.src.Core.BehaviourCore
{
    class Behaviour
    {
        protected float Weight;

        public Behaviour(float weight) {
            this.Weight = weight;
        }

        public virtual void Update(Actor actor) { }
    }

    class BehaviourConstant : Behaviour {
        Vector2 direction;
        public BehaviourConstant(float weight,Vector2 direction)
        : base(weight)
        {
            this.direction = direction;
        }

        public override void Update(Actor actor)
        {
            actor.Direction += this.direction * this.Weight;
        }
    }

    class BehaviourGamepad : Behaviour {
        public BehaviourGamepad(float weight) : base(weight) {
        }
        public override void Update(Actor actor)
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            if (padState.ThumbSticks.Left.Length() > 0) {
                actor.Direction += padState.ThumbSticks.Left * new Vector2(1,-1) * Weight;
            }
        }
    }

    class BehaviourKeyboard : Behaviour
    {
        public BehaviourKeyboard(float weight) : base(weight)
        {
        }
        public override void Update(Actor actor)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Right)) {
                actor.Direction += new Vector2(1, 0) * Weight;
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                actor.Direction += new Vector2(-1, 0) * Weight;
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                actor.Direction += new Vector2(0, -1) * Weight;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                actor.Direction += new Vector2(0, 1) * Weight;
            }
        }
    }

    class BehaviourWander : Behaviour
    {

        static Random random = new Random();
        int tick;
        int changeInterval;
        Vector2 direction;

        public BehaviourWander(float weight,int changeInterval) : base(weight)
        {
            this.changeInterval = changeInterval;
        }


        public override void Update(Actor actor)
        {
            if (tick == 0) {
                direction = Actor.GetRandomDirection();
            }
            tick++;
            tick %= changeInterval;

            actor.Direction += direction * Weight;
        }
    }

    class BehaviourSeek : Behaviour
    {
        Actor target;
        
        public BehaviourSeek(float weight,Actor target) : base(weight)
        {
            this.target = target;
        }


        public override void Update(Actor actor)
        {
            Vector2 targetDirection = target.Position - actor.Position;
            targetDirection.Normalize();
            actor.Direction += targetDirection * Weight;
        }
    }

    class BehaviourAvoid : Behaviour
    {
        Actor target;
        float radius;

        public BehaviourAvoid(float weight, Actor target,float radius) : base(weight)
        {
            this.target = target;
            this.radius = radius;
        }


        public override void Update(Actor actor)
        {
            Vector2 targetDirection = actor.Position - target.Position;

            if (targetDirection.Length() < radius) {

                targetDirection.Normalize();
                actor.Direction += targetDirection * Weight;
            }

        }
    }


}
