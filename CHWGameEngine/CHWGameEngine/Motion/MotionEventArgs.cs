using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHWGameEngine.GameObject;

namespace CHWGameEngine.Motion
{
    public class MotionEventArgs : EventArgs
    {
        public DecimalPoint DistanceMoved { get; set; }
        public double TotalDistanceMoved { get; set; }

        public MotionEventArgs(DecimalPoint distanceMoved, double totalDistanceMoved)
        {
            DistanceMoved = distanceMoved;
            TotalDistanceMoved = totalDistanceMoved;
        }
    }
}
