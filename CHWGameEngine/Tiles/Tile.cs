using System.Drawing;

namespace CHWGameEngine.Tiles
{
    public class Tile
    {
        public static int Width { get; set; }

        public static int Height { get; set; }

        public bool IsSolid { get; set; }
        public int Id { get; private set; }

        public Image Image { get; private set; }

        public Tile(Image image, int id)
        {
            this.Image = image;
            this.Id = id;
            Width = 64;
            Height = 64;
        }

        public Rectangle ToRectangle(int x, int y)
        {
            return new Rectangle(x, y, Width, Height);
        }
    }
}
