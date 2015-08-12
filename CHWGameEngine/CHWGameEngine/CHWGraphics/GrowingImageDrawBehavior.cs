using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CHWGameEngine.GameObject;

namespace CHWGameEngine.CHWGraphics
{
    public delegate void SendSize(Size size);
    public class GrowingImageDrawBehavior : IDrawBehavior
    {
        public Size size;
        public IGameObject GameObject { get; set; }
        public bool IsCircle { get; set; }
        public int SpeedY { get; set; }
        public int SpeedX { get; set; }

        public event SendSize SizeChanged;

        public GrowingImageDrawBehavior(IGameObject gameObject)
        {
            GameObject = gameObject;
            size = new Size(50, 50);
            SpeedX = 1;
            SpeedY = 1;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(GameObject.Image, new Point(
                Convert.ToInt32(GameObject.Location.X),
                Convert.ToInt32(GameObject.Location.Y)));
        }

        public void Draw(Graphics g, Point offset)
        {
            size.Width += SpeedX;
            size.Height += SpeedY;
            if (SizeChanged != null) SizeChanged(size);
            Image newImage = ResizeImage(GameObject.Image, size.Width, size.Height);

            GameObject.MotionBehavior.Area = new Rectangle(
                0 - newImage.Width/2,
                0 - newImage.Height/2,
                newImage.Width,
                newImage.Height);

            g.DrawImage(newImage, new Point(
                Convert.ToInt32(GameObject.Location.X - offset.X - size.Width/2 + GameObject.Image.Size.Width/2),
                Convert.ToInt32(GameObject.Location.Y - offset.Y - size.Height / 2 + GameObject.Image.Size.Height / 2)));
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            return result;
        }
    }
}
