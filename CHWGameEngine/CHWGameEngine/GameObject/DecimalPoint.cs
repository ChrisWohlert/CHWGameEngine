using System;
using System.Drawing;

namespace CHWGameEngine.GameObject
{
    public class DecimalPoint : IComparable<DecimalPoint>
    {
        /// <summary>
        /// The x position
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// The y position
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// DecimalPointer contructor
        /// </summary>
        /// <param name="x">Starting x position</param>
        /// <param name="y">Starting y position</param>
        public DecimalPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// Converts the DecimalPoint to a new Point
        /// </summary>
        /// <returns>Returns a new Point with the same values</returns>
        public Point ToPoint()
        {
            return new Point(Convert.ToInt32(X), Convert.ToInt32(Y));
        }

        public static bool operator <(DecimalPoint left, int right)
        {
            return left.X + left.Y < right;
        }

        public static bool operator >(DecimalPoint left, int right)
        {
            return left.X + left.Y > right;
        }

        public int CompareTo(DecimalPoint other)
        {
            return X + Y > other.X + other.Y ? 1 : -1;
        }
    }
}
