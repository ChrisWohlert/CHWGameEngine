using System.Drawing;
using CHWGameEngine.CHWGraphics;
using CHWGameEngine.GameObject;

namespace CHWGameEngine
{
    public class Game
    {
        #region Properties
        /// <summary>
        /// The graphics engine
        /// </summary>
        private GfxEngine gfx;
        /// <summary>
        /// The GameWorld
        /// </summary>
        private GameWorld gw;
        /// <summary>
        /// The Camera
        /// </summary>
        private Camera camera;
        /// <summary>
        /// The size of the screen
        /// </summary>
        public static Size Size { get; set; }

        public delegate void SendGraphics(Graphics g);

        /// <summary>
        /// Use this event to draw things on top of the background
        /// </summary>
        public event SendGraphics Draw;

        public delegate void SendGameObject(IGameObject gameObject);
        /// <summary>
        /// Event cast when an object moves out of the screen
        /// </summary>
        public event SendGameObject OutOfVision;

        #endregion
        /// <summary>
        /// Game contructor
        /// </summary>
        /// <param name="g">The graphics object to draw onto</param>
        /// <param name="gw">The gameworld</param>
        public Game(Graphics g, GameWorld gw, Size gameSize)
        {
            Size = gameSize;
            this.gw = gw;
            this.camera = new Camera(gw, 0, 0);
            this.gw.Camera = camera;
            gfx = new GfxEngine(g, gw, camera);
            gfx.Draw += gfx_Draw;
            gfx.OutOfVision += gfx_OutOfVision;
        }

        void gfx_OutOfVision(IGameObject gameObject)
        {
            if (OutOfVision != null) OutOfVision(gameObject);
        }

        public void gfx_Draw(Graphics g)
        {
            if (Draw != null) Draw(g);
        }

        public void Render()
        {
            camera.CenterOnObject(gw.Player);
            gfx.Paint();
        }
    }
}
