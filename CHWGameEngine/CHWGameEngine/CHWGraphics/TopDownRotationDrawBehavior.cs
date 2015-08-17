using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using CHWGameEngine.GameObject;

namespace CHWGameEngine.CHWGraphics
{
    public class TopDownRotationDrawBehavior : IDrawBehavior, ICloneable
    {
        /// <summary>
        /// The game object
        /// </summary>
        public IGameObject GameObject { get; set; }
        /// <summary>
        /// The color of the healthbar
        /// </summary>
        public Brush HealthColor { get; set; }
        public bool IsCircle { get; set; }
        /// <summary>
        /// TopDownRotationDrawBehavior constructor
        /// </summary>
        /// <param name="gameObject">gameObjectLocation</param>
        public TopDownRotationDrawBehavior(IGameObject gameObject)
        {
            this.GameObject = gameObject;
            IsCircle = false;
        }


        public void Draw(Graphics g)
        {
            g.DrawImage(RotateImageByAngle(), new Point(
                Convert.ToInt32(GameObject.Location.X),
                Convert.ToInt32(GameObject.Location.Y)));
        }

        public void Draw(Graphics g, Point offset)
        {
            g.DrawImage(RotateImageByAngle(), new Point(
                Convert.ToInt32(GameObject.Location.X - offset.X),
                Convert.ToInt32(GameObject.Location.Y - offset.Y)));
        }

        #region Rotation

        private Bitmap RotateImageByAngle()
        {
            var matrix = new Matrix();
            matrix.Translate((float) GameObject.Image.Width/-2, (float) GameObject.Image.Height/-2, MatrixOrder.Append);
            matrix.RotateAt(GameObject.Angle, new Point(0, 0), MatrixOrder.Append);
            GraphicsPath gp = new GraphicsPath();

            gp.AddPolygon(new[]
                {new Point(0, 0), new Point(GameObject.Image.Width, 0), new Point(0, GameObject.Image.Height)});
            gp.Transform(matrix);
            PointF[] pts = gp.PathPoints;

            Rectangle bBox = BoundingBox(GameObject.Image, matrix);
            GameObject.MotionBehavior.Area = new Rectangle(
                bBox.X + (bBox.Width - GameObject.Image.Size.Width),
                bBox.Y + (bBox.Height - GameObject.Image.Size.Height),
                GameObject.Image.Size.Width,
                GameObject.Image.Size.Height);
            Bitmap newBmp = new Bitmap(bBox.Width, bBox.Height);

            Graphics newG = Graphics.FromImage(newBmp);

            var newMatrix = new Matrix();
            newMatrix.Translate((float) newBmp.Width/2, (float) newBmp.Height/2, MatrixOrder.Append);
            newG.Transform = newMatrix;
            newG.DrawImage(GameObject.Image, pts);
            return newBmp;
        }

        private Rectangle BoundingBox(Image img, Matrix matrix)
        {
            GraphicsUnit gu = new GraphicsUnit();
            Rectangle rImg = Rectangle.Round(img.GetBounds(ref gu));

            // Transform the four points of the image, to get the resized bounding box.
            Point topLeft = new Point(rImg.Left, rImg.Top);
            Point topRight = new Point(rImg.Right, rImg.Top);
            Point bottomRight = new Point(rImg.Right, rImg.Bottom);
            Point bottomLeft = new Point(rImg.Left, rImg.Bottom);
            Point[] points = { topLeft, topRight, bottomRight, bottomLeft };
            GraphicsPath gp = new GraphicsPath(points,
                new[]
                {
                    (byte) PathPointType.Start, (byte) PathPointType.Line, (byte) PathPointType.Line,
                    (byte) PathPointType.Line
                });
            gp.Transform(matrix);
            return Rectangle.Round(gp.GetBounds());
        }

        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
