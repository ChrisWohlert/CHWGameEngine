using System.Drawing;
using CHWGameEngine;
using CHWGameEngine.CHWGraphics;
using CHWGameEngine.GameObject;
using CHWGameEngine.Motion;

namespace TestEngine
{
    class Enemy : IGameObject
    {
        public Enemy(Image image, GameWorld gameWorld)
        {
            Image = image;
            this.MotionBehavior = new NormalMotionBehavior(this, gameWorld);
            Location = new DecimalPoint(300, 150);
            Image = image;
            Angle = 0;
            ActualSpeed = 0;
            Acceleration = 0.3;
            TargetSpeed = 0;
            Inertia = new DecimalPoint(0.5, 0.5);
            DrawBehavior = new TopDownRotationDrawBehavior(this);
            Paralax = Paralax.Middleground;
            IsPhysicalObject = true;
        }

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
        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
