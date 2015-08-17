using System;
using System.Drawing;
using CHWGameEngine.GameObject;

namespace CHWGameEngine.Motion
{
    public delegate void SendDistance(double distance);
    public interface IMotionBehavior : ICloneable
    {
        /// <summary>
        /// Gets or sets if the object can move through walls
        /// </summary>
        bool CanMoveThroughWall { get; set; }
        /// <summary>
        /// Gets or sets if the object can move through other objects
        /// </summary>
        bool CanMoveThroughObjects { get; set; }
        /// <summary>
        /// The object to move
        /// </summary>
        IGameObject GameObject { get; set; }
        /// <summary>
        /// The area that collides
        /// </summary>
        Rectangle Area { get; set; }
        /// <summary>
        /// The speed of the object
        /// </summary>
        DecimalPoint Speed { get; set; }
        /// <summary>
        /// The new speed
        /// </summary>
        DecimalPoint NewSpeed { get; set; }
        /// <summary>
        /// Gets the total distance travelled by the object
        /// </summary>
        double DistanceTravelled { get; }
        /// <summary>
        /// Moves the object
        /// </summary>
        void Move();
        /// <summary>
        /// Move at specified angle
        /// </summary>
        /// <param name="angle">The angle to move at</param>
        void Move(int angle);
        /// <summary>
        /// Moves the object to specified location
        /// </summary>
        /// <param name="destination">The location to move to</param>
        void MoveTo(Point destination);
        /// <summary>
        /// Called when the object collides with a wall, if it cannot move through walls
        /// </summary>
        event EventHandler WallCollision;
        event EventHandler<MotionEventArgs> Moved; 
    }
}
