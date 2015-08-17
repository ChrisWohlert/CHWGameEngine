using CHWGameEngine.GameObject;

namespace CHWGameEngine.Motion
{
    public class StationaryMotionBehavior : MotionBehavior
    {
        public StationaryMotionBehavior(IGameObject gameObject, GameWorld gw) : base(gameObject, gw)
        {
            CanMoveThroughWall = true;
            CanMoveThroughObjects = true;
        }

        protected override void CalcMove()
        {
            Speed.X = 0;
            Speed.Y = 0;
        }
    }
}
