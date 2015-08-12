using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using CHWGameEngine.GameObject;
using CHWGameEngine.Tiles;

namespace CHWGameEngine.Motion
{
    public abstract class MotionBehavior : IMotionBehavior
    {
        /// <summary>
        /// The gameworld
        /// </summary>
        protected GameWorld gw;

        private double distanceTravelled;

        public bool CanMoveThroughWall { get; set; }
        public bool CanMoveThroughObjects { get; set; }
        public IGameObject GameObject { get; set; }
        public Rectangle Area { get; set; }
        public DecimalPoint Speed { get; set; }
        public DecimalPoint NewSpeed { get; set; }

        public double DistanceTravelled
        {
            get { return distanceTravelled; }
            private set
            {
                distanceTravelled = Math.Abs(value);
                if (DistianceTravelledChanged != null) DistianceTravelledChanged(distanceTravelled);
            }
        }

        public event EventHandler WallCollision;
        public event SendDistance DistianceTravelledChanged;

        /// <summary>
        /// MotionBehavior contructor
        /// </summary>
        /// <param name="gameObject">GameObject</param>
        /// <param name="gw">GameWorld</param>
        protected MotionBehavior(IGameObject gameObject, GameWorld gw)
        {
            this.GameObject = gameObject;
            this.gw = gw;
            Speed = new DecimalPoint(0, 0);
            NewSpeed = new DecimalPoint(0, 0);
            CanMoveThroughWall = false;
        }
        /// <summary>
        /// Calculates the speed the object will move at
        /// </summary>
        protected void CalcSpeed()
        {
            if (GameObject.ActualSpeed < 0) GameObject.ActualSpeed = 0;
            if (GameObject.ActualSpeed < GameObject.TargetSpeed)
                GameObject.ActualSpeed += GameObject.Acceleration;
            else if (GameObject.ActualSpeed > GameObject.TargetSpeed)
                GameObject.ActualSpeed -= GameObject.Acceleration;

            NewSpeed.X = GameObject.ActualSpeed * Math.Sin(DegreesToRadians(GameObject.Angle));
            NewSpeed.Y = -(GameObject.ActualSpeed * Math.Cos(DegreesToRadians(GameObject.Angle)));

            CalcInertia();
        }
        /// <summary>
        /// Calculates the inertia of the object
        /// </summary>
        private void CalcInertia()
        {
            if (Speed.X < NewSpeed.X)
            {
                Speed.X += GameObject.Inertia.X;

                if (Speed.X > NewSpeed.X)
                    Speed.X = NewSpeed.X;
            }
            if (Speed.X > NewSpeed.X)
            {
                Speed.X -= GameObject.Inertia.X;

                if (Speed.X < NewSpeed.X)
                    Speed.X = NewSpeed.X;
            }


            if (Speed.Y < NewSpeed.Y)
            {
                Speed.Y += GameObject.Inertia.Y;

                if (Speed.Y > NewSpeed.Y)
                    Speed.Y = NewSpeed.Y;
            }
            if (Speed.Y > NewSpeed.Y)
            {
                Speed.Y -= GameObject.Inertia.Y;

                if (Speed.Y < NewSpeed.Y)
                    Speed.Y = NewSpeed.Y;
            }
        }
        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="angle">The angle to move at</param>
        /// <returns></returns>
        private static double DegreesToRadians(int angle)
        {
            angle += 90;
            return (Math.PI / 180.0) * Convert.ToDouble(angle);
        }

        public void Move()
        {
            CalcSpeed();
            CalcMove();

            if (CanMove())
            {
                GameObject.Location.X += Speed.X;
                GameObject.Location.Y += Speed.Y;
                DistanceTravelled += Math.Abs(Speed.X) + Math.Abs(Speed.Y);
            }
            else
            {
                Unstuck();
                Speed.X = 0;
                Speed.Y = 0;
            }
        }
        /// <summary>
        /// Checks if the object can move, or if it is blocked by a wall
        /// </summary>
        /// <returns>Returns true if it is not blocked</returns>
        protected bool CanMove()
        {
            return !IsWall() && !IsObject();
        }

        private bool IsObject()
        {
            if (CanMoveThroughObjects) return false;
            var list = new List<IGameObject>();
            list.Add(GameObject);

            if (
                gw.GetClosestGameObjectFromMap(new Point((int) (GameObject.Location.X + Speed.X), (int) (GameObject.Location.Y +  Speed.Y)),
                    Area.Width + 10, list) == null)
                return false;

            return true;
        }

        public virtual void Unstuck()
        {
            for (int i = 1; i < 180; i++)
            {
                GameObject.Angle += i;
                if (CanMove()) break;
                i++;
                GameObject.Angle -= i;
                if (CanMove()) break;
            }
        }

        private bool IsWall()
        {
            if (CanMoveThroughWall) return false;

            int x = (int)(GameObject.Location.X + Area.X + Speed.X) / Tile.Width;
            int y = (int)(GameObject.Location.Y + Area.Y + Speed.Y) / Tile.Height;

            int xEnd = (int)(GameObject.Location.X + Area.X + Speed.X + Area.Width) / Tile.Width;
            int yEnd = (int)(GameObject.Location.Y + Area.Y + Speed.Y + Area.Height) / Tile.Height;

            if (!gw.GetTile(x, y).IsSolid
                && !gw.GetTile(xEnd, y).IsSolid
                && !gw.GetTile(x, yEnd).IsSolid
                && !gw.GetTile(xEnd, yEnd).IsSolid)
                return false;

            if (WallCollision != null)
                WallCollision(this, EventArgs.Empty);

            return true;
        }

        public void MoveTo(Point destination)
        {
            Point location = GameObject.Location.ToPoint();
            int rightOrLeft = location.X < destination.X ? 1 : -1;
            int upOrDown = location.Y < destination.Y ? 1 : -1;
            while ((GameObject.Location.X*rightOrLeft) < (destination.X*rightOrLeft) &&
                    (GameObject.Location.Y*upOrDown) < (destination.Y*upOrDown))
            {
                Move();
            }
        }


        public void Move(int angle)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Calculates the movement
        /// </summary>
        protected abstract void CalcMove();

        public virtual object Clone()
        {
            var clone = MemberwiseClone() as IMotionBehavior;

            return clone;
        }
    }
}
