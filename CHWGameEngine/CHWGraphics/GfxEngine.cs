using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using CHWGameEngine.Tiles;

namespace CHWGameEngine.CHWGraphics
{
    class GfxEngine
    {
        #region Fields

        /// <summary>
        /// The graphics object to draw onto
        /// </summary>
        private Graphics g;
        /// <summary>
        /// The game world
        /// </summary>
        private GameWorld gw;
        /// <summary>
        /// The camera
        /// </summary>
        private Camera camera;
        /// <summary>
        /// The Bitmap to draw onto, before drawing it to g
        /// </summary>
        private Bitmap bmp;
        /// <summary>
        /// The graphics object to draw onto bmp before g draws it
        /// </summary>
        private Graphics frameG;

        public delegate void SendGraphics(Graphics g);
        /// <summary>
        /// Use this event to draw things on top of the background
        /// </summary>
        public event SendGraphics Draw;
        /// <summary>
        /// Event cast when an object moves outside the screen
        /// </summary>
        public event Game.SendGameObject OutOfVision;
        #endregion
        /// <summary>
        /// GfxEngine constructor
        /// </summary>
        /// <param name="g">Graphics object to draw onto</param>
        /// <param name="gw">GameWorld</param>
        /// <param name="camera">Camera</param>
        public GfxEngine(Graphics g, GameWorld gw, Camera camera)
        {
            this.g = g;
            this.gw = gw;
            this.camera = camera;
            Bitmap original = new Bitmap(Game.Size.Width, Game.Size.Height);
            bmp = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppPArgb);
            frameG = Graphics.FromImage(bmp);
        }
        /// <summary>
        /// Paints the background
        /// </summary>
        private void PaintBackground()
        {
            int xStart = (int)Math.Max(0, camera.XOffset / Tile.Width);
            int xEnd = (int)Math.Min(gw.Tiles.GetLength(0), (camera.XOffset + Game.Size.Width) / Tile.Width + 1);
            int yStart = (int)Math.Max(0, camera.YOffset / Tile.Height); ;
            int yEnd = (int)Math.Min(gw.Tiles.GetLength(1), (camera.YOffset + Game.Size.Height) / Tile.Height + 1);

            for (int x = xStart; x < xEnd; x++)
                for (int y = yStart; y < yEnd; y++)
                {
                    Tile t = gw.GetTile(x, y);
                    int tileOffsetX = (int)(x * Tile.Width - camera.XOffset);
                    int tileOffsetY = (int)(y * Tile.Height - camera.YOffset);
                    frameG.DrawImage(t.Image, tileOffsetX, tileOffsetY, Tile.Width, Tile.Height);
                }
        }
        /// <summary>
        /// Paints everything in camera's view
        /// </summary>
        public void Paint()
        {
            frameG.CompositingMode = CompositingMode.SourceCopy;
            frameG.InterpolationMode = InterpolationMode.NearestNeighbor;
            PaintBackground();
            frameG.CompositingMode = CompositingMode.SourceOver;
            gw.VisibleGameObjects.Clear();

            try
            {
                foreach (var go in gw.GameObjects.OrderBy(x => x.Paralax))
                    if (camera.IsOnScreen(go))
                    {
                        go.DrawBehavior.Draw(frameG,
                            new Point((int)camera.XOffset + go.Image.Size.Width / 2, (int)camera.YOffset + go.Image.Size.Height / 2));
                        gw.VisibleGameObjects.Add(go);
                    }
                    else
                    {
                        if (OutOfVision != null) OutOfVision(go);
                    }
            }
            catch (Exception)
            {
                // ignored
            }

            if (Draw != null) Draw(frameG);
            g.DrawImage(bmp, 0, 0);
        }
    }
}
