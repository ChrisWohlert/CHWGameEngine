using CHWGameEngine.GameObject;

namespace CHWGameEngine.Motion
{
    public class OffensiveMotionBehavior : MotionBehavior
    {
        public DecimalPoint TargetLocation { get; set; }

        public OffensiveMotionBehavior(IGameObject gameObject, GameWorld gameWorld, DecimalPoint targetLocation)
            : base(gameObject, gameWorld)
        {
            this.TargetLocation = targetLocation;
        }

        protected override void CalcMove()
        {
            GameObject.Angle = (int) GameWorld.CalcAngle(GameObject.Location.ToPoint(), TargetLocation.ToPoint());
        }
    }
}
