using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using CHWGameEngine.GameObject;
using CHWGameEngine.Tiles;

namespace CHWGameEngine.Motion
{
    public class NormalMotionBehavior : IMotionBehavior
    {
        private IGameObject go;
        private GameWorld gw;

        public NormalMotionBehavior(IGameObject gameObject, GameWorld gameWorld)
        {
            this.go = gameObject;
            this.gw = gameWorld;
        }

        private void CalcSpeed()
        {
            if (go.ActualSpeed < go.TargetSpeed)
                go.ActualSpeed += go.Acceleration;
            else if (go.ActualSpeed > go.TargetSpeed)
                go.ActualSpeed -= go.Acceleration;

            go.NewSpeed.X = go.ActualSpeed * Math.Sin(DegreesToRadians(go.Angle));
            go.NewSpeed.Y = -(go.ActualSpeed * Math.Cos(DegreesToRadians(go.Angle)));

            CalcInertia();
        }

        private void CalcInertia()
        {
            if (go.Speed.X < go.NewSpeed.X)
            {
                go.Speed.X += go.Inertia.X;

                if (go.Speed.X > go.NewSpeed.X)
                    go.Speed.X = go.NewSpeed.X;
            }
            if (go.Speed.X > go.NewSpeed.X)
            {
                go.Speed.X -= go.Inertia.X;

                if (go.Speed.X < go.NewSpeed.X)
                    go.Speed.X = go.NewSpeed.X;
            }


            if (go.Speed.Y < go.NewSpeed.Y)
            {
                go.Speed.Y += go.Inertia.Y;

                if (go.Speed.Y > go.NewSpeed.Y)
                    go.Speed.Y = go.NewSpeed.Y;
            }
            if (go.Speed.Y > go.NewSpeed.Y)
            {
                go.Speed.Y -= go.Inertia.Y;

                if (go.Speed.Y < go.NewSpeed.Y)
                    go.Speed.Y = go.NewSpeed.Y;
            }
        }

        private static double DegreesToRadians(int angle)
        {
            angle += 90;
            return (Math.PI / 180.0) * Convert.ToDouble(angle);
        }

        private static int RadiansToDegree(double radians)
        {
            return Convert.ToInt32((180.0 / Math.PI) * radians);
        }

        public void Move()
        {
            CalcSpeed();
            int x = (int)(go.Location.X + go.Speed.X) / Tile.Width;
            int y = (int)(go.Location.Y + go.Speed.Y) / Tile.Height;

            int xEnd = ((int)(go.Location.X + go.Speed.X) + go.Area.Right) / Tile.Width;
            int yEnd = ((int)(go.Location.Y + go.Speed.Y) + go.Area.Bottom) / Tile.Height;
            
            
            if (!gw.GetTile(x, y).IsSolid
                || !gw.GetTile(xEnd, y).IsSolid
                || !gw.GetTile(x, yEnd).IsSolid
                || !gw.GetTile(xEnd, yEnd).IsSolid)
            {
                go.Location.X += go.Speed.X;
                go.Location.Y += go.Speed.Y;   
            }
        }

        private Rectangle GetTileRectangle(int x, int y)
        {
            return gw.GetTile(x, y).ToRectangle(x, y);
        }

        public void MoveTo(Point destination)
        {
            Point location = go.Location.ToPoint();
            int rightOrLeft = location.X < destination.X ? 1 : -1;
            int upOrDown = location.Y < destination.Y ? 1 : -1;
            while ((go.Location.X*rightOrLeft) < (destination.X*rightOrLeft) &&
                    (go.Location.Y*upOrDown) < (destination.Y*upOrDown))
            {
                Move();
                Thread.Sleep(1000/Game.Fps);
            }
        }
    }
}
