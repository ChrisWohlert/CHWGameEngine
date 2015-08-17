using System.Drawing;
using CHWGameEngine.GameObject;

namespace CHWGameEngine
{
    class Camera
    {
        /// <summary>
        /// The x distance the camera is off 
        /// </summary>
        public float XOffset { get; set; }
        /// <summary>
        /// The y distance the camera is off
        /// </summary>
        public float YOffset { get; set; }
        /// <summary>
        /// Gets a rectangle matching the camera's view
        /// </summary>
        public Rectangle Screen
        {
            get
            {
                int x = (int)this.XOffset;
                int y = (int)this.YOffset;
                int width = Game.Size.Width;
                int height = Game.Size.Height;
                return new Rectangle(x, y, width, height);
            }
        }
        /// <summary>
        /// The gameworld
        /// </summary>
        private GameWorld gw;
        /// <summary>
        /// Camera constructor
        /// </summary>
        /// <param name="gw">GameWorld</param>
        /// <param name="xOffset">Start x offset</param>
        /// <param name="yOffset">Start y offset</param>
        public Camera(GameWorld gw, float xOffset, int yOffset)
        {
            this.gw = gw;
            XOffset = xOffset;
            YOffset = yOffset;
        }
        /// <summary>
        /// Moves the camera
        /// </summary>
        /// <param name="dx">The distance to move in x direction, in pixels</param>
        /// <param name="dy">The distance to move in y direction, in pixels</param>
        public void Move(float dx, float dy)
        {
            XOffset += dx;
            YOffset += dy;
        }
        /// <summary>
        /// Centers the camera on the object
        /// </summary>
        /// <param name="gameObject">The object to center on</param>
        public void CenterOnObject(IGameObject gameObject)
        {
            XOffset = (float)gameObject.Location.X - Game.Size.Width / 2;
            YOffset = (float)gameObject.Location.Y - Game.Size.Height / 2;
        }
        /// <summary>
        /// Checks if object is on screen
        /// </summary>
        /// <param name="gameObject">The object to check</param>
        /// <returns>Returns true if the object is on screen, false otherwise</returns>
        public bool IsOnScreen(IGameObject gameObject)
        {
            return this.Screen.IntersectsWith(new Rectangle(gameObject.Location.ToPoint(), gameObject.Image.Size));
        }
    }
}
