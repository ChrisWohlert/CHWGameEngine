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

        private double angle;

        public CircleMotionBehavior(IGameObject gameObject, GameWorld gw, DecimalPoint center) : base(gameObject, gw)
        {
            Center = center;
            Range = 200;
            angle = 0;
        }

        protected override void CalcMove()
        {
            GameObject.Location.X = (Center.X + 100 * Math.Cos(angle));
            GameObject.Location.Y = (Center.Y + 100 * Math.Sin(angle));

            double speed = 2*Math.PI*1;
            angle -= speed / (GameObject.ActualSpeed * 10);
            GameObject.Angle = GameWorld.CalcAngle(GameObject.Location.ToPoint(), Center.ToPoint()) + 90;
        }
    }
}
