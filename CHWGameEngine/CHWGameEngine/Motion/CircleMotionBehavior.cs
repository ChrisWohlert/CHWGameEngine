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
        public double Range { get; set; }
        private double angle;
        private double range;

        public CircleMotionBehavior(IGameObject gameObject, GameWorld gw, DecimalPoint center) : base(gameObject, gw)
        {
            Center = center;
            range = gw.GetDistance(center, gameObject.Location);
            Range = 180;
            angle = GameWorld.CalcAngle(GameObject.Location.ToPoint(), Center.ToPoint());
        }

        protected override void CalcMove()
        {
            if (range < Range) range += 2;
            GameObject.Location.X = (Center.X + range * Math.Cos(angle));
            GameObject.Location.Y = (Center.Y + range * Math.Sin(angle));

            double speed = 2 * Math.PI * 1;
            angle -= speed / (GameObject.ActualSpeed * 10);
            GameObject.Angle = (int)GameWorld.CalcAngle(GameObject.Location.ToPoint(), Center.ToPoint()) + 90;
        }
    }
}
