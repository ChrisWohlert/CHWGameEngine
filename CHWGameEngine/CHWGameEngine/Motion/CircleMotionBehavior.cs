using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHWGameEngine.GameObject;

namespace CHWGameEngine.Motion
{
     public class CircleMotionBehavior : MotionBehavior
    {
        public DecimalPoint Center { get; set; }
        public int Range { get; set; }

        public CircleMotionBehavior(IGameObject gameObject, GameWorld gw, DecimalPoint center) : base(gameObject, gw)
        {
            Center = center;
            Range = 200;
        }

        protected override void CalcMove()
        {
            GameObject.Angle = (int)GameWorld.CalcAngle(GameObject.Location.ToPoint(), Center.ToPoint()) + 90;
            double deltaX = (GameObject.Location.X - Center.X);
            double deltaY = (GameObject.Location.Y - Center.Y);

            double radius = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            double curTheta = Math.Atan2(deltaX, deltaY);
            double deltaTheta = GameObject.ActualSpeed + 5 / radius;
            double newTheta = curTheta + deltaTheta;

            double newDeltaX = radius*Math.Cos(newTheta);
            double newDeltaY = radius*Math.Sin(newTheta);

            GameObject.Location.X = Center.X + newDeltaX;
            GameObject.Location.Y = Center.Y + newDeltaY;
        }
    }
}
