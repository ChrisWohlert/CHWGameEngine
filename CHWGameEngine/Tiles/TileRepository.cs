using System.Collections.Generic;

namespace CHWGameEngine.Tiles
{
    public static class TileRepository
    {
        private static List<Tile> tiles;

        internal static List<Tile> Tiles
        {
            get { return tiles ?? (tiles = new List<Tile>()); }
        }

        public static void Add(Tile t)
        {
            Tiles.Insert(t.Id, t);
        }
    }
}
