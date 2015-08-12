using System.Drawing;
using CHWGameEngine;
using CHWGameEngine.CHWGraphics;
using CHWGameEngine.GameObject;
using CHWGameEngine.Motion;

namespace TestEngine
{
    class Player : IGameObject
    {
        public DecimalPoint Location { get; set; }

        public Image Image { get; set; }

        public int Angle { get; set; }

        public double ActualSpeed { get; set; }

        public double Acceleration { get; set; }

        public double TargetSpeed { get; set; }

        public DecimalPoint Inertia { get; set; }
        public Paralax Paralax { get; set; }

        public IMotionBehavior MotionBehavior { get; set; }
        public bool IsPhysicalObject { get; set; }
        public IDrawBehavior DrawBehavior { get; set; }

        public Player(Image image, GameWorld gameWorld)
        {
            double speed = 0.000001;
            this.MotionBehavior = new NormalMotionBehavior(this, gameWorld);
            MotionBehavior.CanMoveThroughWall = false;
            Location = new DecimalPoint(300, 150);
            Image = image;
            Angle = 0;
            ActualSpeed = 0;
            Acceleration = 0.3;
            TargetSpeed = 0;
            Inertia = new DecimalPoint(0.5, 0.5);
            DrawBehavior = new TopDownRotationDrawBehavior(this);
            //((GrowingImageDrawBehavior)DrawBehavior).SpeedX = 10;
            //((GrowingImageDrawBehavior)DrawBehavior).SpeedY = 10;
            //((GrowingImageDrawBehavior) DrawBehavior).SizeChanged += (Size) =>
            //{
            //    if (Size.Width > 400)
            //    {
            //        ((GrowingImageDrawBehavior)DrawBehavior).SpeedX = -2;
            //        ((GrowingImageDrawBehavior)DrawBehavior).SpeedY = -2;
            //    }

            //    if (((GrowingImageDrawBehavior) DrawBehavior).SpeedY < 0)
            //    {
            //            ((GrowingImageDrawBehavior)DrawBehavior).SpeedX = 0 - (int)speed;
            //            ((GrowingImageDrawBehavior)DrawBehavior).SpeedY = 0 - (int)speed;
            //            speed += speed;
            //    }

            //};


            Paralax = Paralax.Middleground;
            IsPhysicalObject = true;
        }

        public void Move()
        {
            MotionBehavior.Move();
        }

        public void Attack(IGameObject spell)
        {
            spell.ActualSpeed = this.ActualSpeed;
            spell.Angle = this.Angle;
            spell.Location = new DecimalPoint(this.Location.X, this.Location.Y);
        }

        public void Break()
        {
            this.TargetSpeed = 0;
            MotionBehavior.Move();
        }

        public void MoveTo(Point destination)
        {
            MotionBehavior.MoveTo(destination);
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
