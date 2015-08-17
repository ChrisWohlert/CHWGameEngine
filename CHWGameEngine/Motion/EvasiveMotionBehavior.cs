using CHWGameEngine.GameObject;

namespace CHWGameEngine.Motion
{
    public class EvasiveMotionBehavior : MotionBehavior
    {
        public DecimalPoint TargetLocation { get; set; }

        private double targetSpeed;
        public int MinRange { get; set; }
        public int MidRange { get; set; }
        public int MaxRange { get; set; }

        public EvasiveMotionBehavior(IGameObject gameObject, GameWorld gw, DecimalPoint targetLocation) : base(gameObject, gw)
        {
            TargetLocation = targetLocation;
            MinRange = 200;
            MidRange = 400;
            MaxRange = 1000;
            this.targetSpeed = gameObject.TargetSpeed;
        }

        protected override void CalcMove()
        {
            if (GameWorld.IsInRange(GameObject.Location, TargetLocation, MaxRange))
            {
                int angle = (int) GameWorld.CalcAngle(GameObject.Location.ToPoint(), TargetLocation.ToPoint());

                if (GameWorld.IsInRange(GameObject.Location, TargetLocation, MinRange))
                {
                    if (angle < 180)
                        angle += 180;
                    else
                        angle -= 180;

                    GameObject.TargetSpeed = this.targetSpeed;
                }
                else if (GameWorld.IsInRange(GameObject.Location, TargetLocation, MinRange, MidRange))
                {
                    GameObject.TargetSpeed = 0;
                }
                else
                {
                    GameObject.TargetSpeed = this.targetSpeed;
                }

                GameObject.Angle = angle;
            }
            else
            {
                Speed.X = 0;
                Speed.Y = 0;
            }
        }
    }
}
