using System;
using System.Drawing;
using CHWGameEngine.GameObject;

namespace CHWGameEngine.CHWGraphics
{
    public interface IDrawBehavior : ICloneable
    {
        /// <summary>
        /// Gets or sets the object to draw
        /// </summary>
        IGameObject GameObject { get; set; }
        /// <summary>
        /// Gets whether the object is a circle
        /// </summary>
        bool IsCircle { get; set; }
        /// <summary>
        /// Draws the object
        /// </summary>
        /// <param name="g">The graphics object to draw onto</param>
        void Draw(Graphics g);
        /// <summary>
        /// Draws the object
        /// </summary>
        /// <param name="g">The graphics object to draw onto</param>
        /// <param name="offset">The offset to draw at</param>
        void Draw(Graphics g, Point offset);
    }
}
