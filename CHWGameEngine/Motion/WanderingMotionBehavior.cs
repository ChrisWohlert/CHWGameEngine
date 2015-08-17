using System;
using CHWGameEngine.GameObject;

namespace CHWGameEngine.Motion
{
    public class WanderingMotionBehavior : MotionBehavior
    {
        private int moveCount;
        private int maxMoveCount;
        public WanderingMotionBehavior(IGameObject gameObject, GameWorld gw) : base(gameObject, gw)
        {
            moveCount = 0;
            maxMoveCount = 50;
        }

        protected override void CalcMove()
        {
            Speed.X /= 1.5;
            Speed.Y /= 1.5;
            int newAngle = new Random().Next(360);
            if (moveCount++ > maxMoveCount)
            {
                if (GameObject.Angle < 180)
                    GameObject.Angle += newAngle;
                else
                    GameObject.Angle -= newAngle;

                moveCount = 0;
            }
        }
    }
}
