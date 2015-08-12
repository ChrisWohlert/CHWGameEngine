using System;
using System.Drawing;
using System.Threading.Tasks;
using CHWGameEngine;
using CHWGameEngine.CHWGraphics;
using CHWGameEngine.GameObject;
using CHWGameEngine.Motion;

namespace TestEngine
{
    class Fireball : IGameObject
    {
        private GameWorld gw;
        private IGameObject source;
        private IGameObject destination;
        private IMotionBehavior motionBehavior;

        public Fireball(Image image, GameWorld gameWorld, IGameObject source)
        {
            this.source = source;
            gw = gameWorld;
            Image = image;
            Init();
        }

        private void Init()
        {
            gw.Collision += gw_Collision;
            Acceleration = 2;
            Inertia = new DecimalPoint(5, 5);
            TargetSpeed = 30;
            MotionBehavior = new NormalMotionBehavior(this, gw);
            MotionBehavior.WallCollision += MotionBehavior_WallCollision;
            gw.GameObjects.Add(this);
            DrawBehavior = new TopDownRotationDrawBehavior(this);
            Paralax = Paralax.Foreground;
            IsPhysicalObject = false;
        }

        void gw_Collision(ref IGameObject object1, ref IGameObject object2)
        {
            if(object1 == this && object2 != source)
                Remove();
            else if (object2 == this && object1 != source)
                Remove();
        }

        void MotionBehavior_WallCollision(object sender, EventArgs e)
        {
            Remove();
        }

        private void Remove()
        {
            Task t = new Task(() => gw.GameObjects.Remove(this));
            t.Start();
        }

        public void Move()
        {
            TargetSpeed = 30;
            MotionBehavior.Move();
        }

        public DecimalPoint Location { get; set; }
        public Image Image { get; set; }
        public int Angle { get; set; }
        public double ActualSpeed { get; set; }
        public double Acceleration { get; set; }
        public double TargetSpeed { get; set; }
        public DecimalPoint Inertia { get; set; }
        public Paralax Paralax { get; set; }

        public IMotionBehavior MotionBehavior
        {
            get { return motionBehavior; }
            set
            {
                motionBehavior = value;
                motionBehavior.WallCollision += MotionBehavior_WallCollision;
            }
        }

        public bool IsPhysicalObject { get; set; }
        public IDrawBehavior DrawBehavior { get; set; }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
