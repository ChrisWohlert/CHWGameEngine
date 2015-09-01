using System;
using System.Drawing;
using CHWGameEngine.CHWGraphics;
using CHWGameEngine.Motion;

namespace CHWGameEngine.GameObject
{
    public enum Paralax { Background, Middleground, Foreground }
    public interface IGameObject : ICloneable
    {
        /// <summary>
        /// The location of the object
        /// </summary>
        DecimalPoint Location { get; set; }
        /// <summary>
        /// The image of the object
        /// </summary>
        Image Image { get; set; }
        /// <summary>
        /// The angle the object is facing
        /// </summary>
        double Angle { get; set; }
        /// <summary>
        /// The actual speed of the object
        /// </summary>
        double ActualSpeed { get; set; }
        /// <summary>
        /// The rate the object will take on speed
        /// </summary>
        double Acceleration { get; set; }
        /// <summary>
        /// The maximum speed
        /// </summary>
        double TargetSpeed { get; set; }
        /// <summary>
        /// The rate of which the object can turn around
        /// </summary>
        DecimalPoint Inertia { get; set; }
        /// <summary>
        /// Determines where the object is, the order it is drawn
        /// </summary>
        Paralax Paralax { get; set; }
        /// <summary>
        /// The motion bahavior of the object
        /// </summary>
        IMotionBehavior MotionBehavior { get; set; }
        /// <summary>
        /// The motion bahavior of the object
        /// </summary>
        bool IsPhysicalObject { get; set; }
        /// <summary>
        /// The draw behavior of the object
        /// </summary>
        IDrawBehavior DrawBehavior { get; set; }
    }
}
