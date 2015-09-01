using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CHWGameEngine.GameObject;
using CHWGameEngine.Tiles;

namespace CHWGameEngine
{
    public class GameWorld
    {
        #region Properties
        /// <summary>
        /// The controllable player
        /// </summary>
        public IGameObject Player { get; set; }
        /// <summary>
        /// A list of gameobject
        /// </summary>
        public List<IGameObject> GameObjects { get; private set; }
        /// <summary>
        /// A list of gameobjects, that are currently inside the screen
        /// </summary>
        public List<IGameObject> VisibleGameObjects { get; private set; }
        /// <summary>
        /// Gets the offset of the screen in the x direction
        /// </summary>
        public float OffsetX { get { return Camera.XOffset; } }
        /// <summary>
        /// Gets the offset of the screen in the y direction
        /// </summary>
        public float OffsetY { get { return Camera.YOffset; } }

        private Dictionary<IGameObject, List<IGameObject>> collidingObjects;

        #endregion

        #region Events

        public delegate void CollistionEvents(ref IGameObject object1, ref IGameObject object2);
        /// <summary>
        /// Event called when two gameobjects collide
        /// </summary>
        public event CollistionEvents Collision;
        public event CollistionEvents Split;

        #endregion

        #region Fields
        /// <summary>
        /// The tiles that make up the map
        /// </summary>
        public int[,] Tiles { get; private set; }
        internal Camera Camera { get; set; }

        #endregion
        /// <summary>
        /// GameWorld constructor
        /// </summary>
        /// <param name="path">Filepath of the map</param>
        public GameWorld(string path)
        {
            LoadWorld(path);
            this.GameObjects = new List<IGameObject>();
            this.VisibleGameObjects = new List<IGameObject>();
            collidingObjects = new Dictionary<IGameObject, List<IGameObject>>();
        }

        #region Methods
        /// <summary>
        /// Loads the map from file
        /// </summary>
        /// <param name="path">Filepath of the map</param>
        private void LoadWorld(string path)
        {
            var sr = new StreamReader(new FileStream(path, FileMode.Open));
            string file = sr.ReadToEnd();
            file = file.Replace("\r", "");
            file = file.Replace("\n", "\t");
            var stringTokens = file.Split('\t');

            int[] tokens = Utils.ParseStringArray(stringTokens);

            int width = tokens[0];
            int height = tokens[1];

            Tiles = new int[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Tiles[x, y] = tokens[(x + y * width) + 2];
        }
        /// <summary>
        /// Checks if any collisions are happening
        /// </summary>
        public void CheckCollisions()
        {
            if (Collision != null)
            {
                for (int i = 0; i < VisibleGameObjects.Count; i++)
                {
                    for (int j = i + 1; j < VisibleGameObjects.Count; j++)
                    {
                        var obj1 = VisibleGameObjects[i];
                        var obj2 = VisibleGameObjects[j];
                        if (IsColliding(obj1, obj2))
                        {
                            if (!collidingObjects.ContainsKey(obj1))
                                collidingObjects.Add(obj1, new List<IGameObject>());

                            if (!collidingObjects[obj1].Contains(obj2))
                            {
                                collidingObjects[obj1].Add(obj2);
                                Collision(ref obj1, ref obj2);
                            }
                        }
                        else
                        {
                            if (collidingObjects.ContainsKey(obj1))
                            {
                                if (collidingObjects[obj1].Contains(obj2))
                                {
                                    collidingObjects[obj1].Remove(obj2);
                                    Split(ref obj1, ref obj2);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Checks if two objects are intersecting
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public bool IsColliding(IGameObject obj1, IGameObject obj2)
        {
            try
            {
                Rectangle obj1Rec = new Rectangle(obj1.Location.ToPoint(), new Size(obj1.MotionBehavior.Area.Width, obj1.MotionBehavior.Area.Height + 100));
                Rectangle obj2Rec = new Rectangle(obj2.Location.ToPoint(), new Size(obj1.MotionBehavior.Area.Width, obj1.MotionBehavior.Area.Height + 100));

                if (obj1.DrawBehavior.IsCircle || obj2.DrawBehavior.IsCircle)
                    return IsInRange(obj1.Location, obj2.Location, obj1.MotionBehavior.Area.Width / 2 + obj2.MotionBehavior.Area.Width / 2);

                return obj1Rec.IntersectsWith(obj2Rec);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
        /// <summary>
        /// Gets the tile at specified coordinate
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns></returns>
        public Tile GetTile(int x, int y)
        {
            Tile t = TileRepository.Tiles[Tiles[x, y]];

            return t;
        }
        /// <summary>
        /// Gets the gameobject at specified location
        /// </summary>
        /// <param name="location">The location on screen</param>
        /// <returns>Returns the object, or null if nothing is found</returns>
        public IGameObject GetIGameObjectFromScreen(Point location)
        {
            Rectangle rec = new Rectangle(location, new Size(1, 1));
            try
            {
                var obj = GameObjects.FirstOrDefault(x => new Rectangle(
                    (int)(x.Location.X - Camera.XOffset + x.MotionBehavior.Area.X), (int)(x.Location.Y - Camera.YOffset + x.MotionBehavior.Area.Y),
                    x.MotionBehavior.Area.Width, x.MotionBehavior.Area.Height).IntersectsWith(rec));
                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// Gets the gameobject at specified location on the map
        /// </summary>
        /// <param name="location">The location on screen</param>
        /// <returns>Returns the object, or null if nothing is found</returns>
        public IGameObject GetIGameObjectFromMap(Point location)
        {
            Rectangle rec = new Rectangle(location, new Size(1, 1));
            try
            {
                var obj = GameObjects.FirstOrDefault(x => new Rectangle(
                    (int)(x.Location.X + x.MotionBehavior.Area.X), (int)(x.Location.Y + x.MotionBehavior.Area.Y),
                    x.MotionBehavior.Area.Width, x.MotionBehavior.Area.Height).IntersectsWith(rec));
                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        public DecimalPoint GetDistance(DecimalPoint location, DecimalPoint destination)
        {
            return new DecimalPoint(Math.Abs(location.X - destination.X), Math.Abs(location.Y - destination.Y));
        }

        public IEnumerable<IGameObject> GetClosestGameObjects(DecimalPoint location, int range)
        {
            return GameObjects.Where(x => GetDistance(location, x.Location) < range && x.IsPhysicalObject);
        }

        public IGameObject GetClosestGameObjectFromMap(Point location, int maxRange = 2000, IEnumerable<IGameObject> excluded = null)
        {
            return GetClosestGameObject(new Point(location.X - (int)Camera.XOffset, location.Y - (int)Camera.YOffset), excluded, maxRange);
        }

        public IGameObject GetClosestGameObject(Point location, IEnumerable<IGameObject> excluded, int maxRange = 2000)
        {
            IGameObject closest = null;
            List<IGameObject> list = VisibleGameObjects.Where(x => x.IsPhysicalObject).ToList();
            List<IGameObject> excludedList = new List<IGameObject>();
            if (excluded != null)
            {
                excludedList = excluded.ToList();
            }
            else
            {
                excludedList.Add(Player);
            }


            try
            {
                foreach (var o in list)
                {
                    int xDistance = (int)(Math.Abs(location.X - (o.Location.X - Camera.XOffset)));
                    int yDistance = (int)(Math.Abs(location.Y - (o.Location.Y - Camera.YOffset)));
                    if (xDistance + yDistance < maxRange && !excludedList.Contains(o))
                    {
                        closest = o;
                        maxRange = xDistance + yDistance;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return closest;
        }
        /// <summary>
        /// Removes object from the game
        /// </summary>
        /// <param name="gameObject"></param>
        public void Remove(IGameObject gameObject)
        {
            if (GameObjects.Contains(gameObject))
                GameObjects.Remove(gameObject);

            if (VisibleGameObjects.Contains(gameObject))
                VisibleGameObjects.Remove(gameObject);

            if (collidingObjects.ContainsKey(gameObject))
                collidingObjects.Remove(gameObject);

            foreach (var go in collidingObjects)
            {
                if (go.Value.Contains(gameObject))
                    go.Value.Remove(gameObject);
            }
        }

        #endregion

        #region RangeChecks
        /// <summary>
        /// Checks if object is within the spcified range
        /// </summary>
        /// <param name="range">The range to check</param>
        /// <returns>Returns true if the object is within range</returns>
        public static bool IsInRange(DecimalPoint source, DecimalPoint destination, int range)
        {
            bool x = Math.Abs(source.X - destination.X) < range;
            bool y = Math.Abs(source.Y - destination.Y) < range;

            return x && y;
        }
        /// <summary>
        /// Checks if the object is within two ranges
        /// </summary>
        /// <param name="minRange">Minimum range</param>
        /// <param name="maxRange">Maximum range</param>
        /// <returns>Returns true if the object is within the two ranges</returns>
        public static bool IsInRange(DecimalPoint source, DecimalPoint destination, int minRange, int maxRange)
        {
            double x = Math.Abs(source.X - destination.X);
            double y = Math.Abs(source.Y - destination.Y);

            bool inX = x > minRange && x < maxRange;
            bool inY = y > minRange && x < maxRange;

            return inX || inY;
        }

        #endregion

        /// <summary>
        /// Calculates the angle between two points
        /// </summary>
        /// <param name="first">The first point</param>
        /// <param name="second">The second point</param>
        /// <returns>Returns the angle the first should move to get to the second</returns>
        public static double CalcAngle(Point first, Point second)
        {
            float dx = second.X - first.X;
            float dy = second.Y - first.Y;
            return Math.Atan2(dy, dx) * 180 / Math.PI;
        }
    }
}
