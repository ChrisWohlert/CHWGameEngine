using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CHWGameEngine;
using CHWGameEngine.GameObject;
using CHWGameEngine.Motion;
using CHWGameEngine.Tiles;
using res = TestEngine.Properties.Resources;

namespace TestEngine
{
    public partial class Form1 : Form
    {
        private Player p;
        private Enemy p2;
        private Game game;
        private GameWorld gw = new GameWorld(@"C:\Users\54430\Google Drev\level1.txt");
        private Thread t;
        private Point dest;

        public Form1()
        {
            InitializeComponent();

            p = new Player(res.Player, gw);
            p2 = new Enemy(res.Player, gw) { Location = new DecimalPoint(3050, 3150) };
            gw.Player = p;
            p.Location.X = 3000;
            p.Location.Y = 3000;

            p2.TargetSpeed = 6;
            p2.Angle = 180;
            p2.MotionBehavior = new CircleMotionBehavior(p2, gw, p.Location);
            Tile grass = new Tile(res.Grass, 0);
            grass.IsSolid = false;
            Tile rock = new Tile(res.Rock, 1);
            rock.IsSolid = true;
            Tile Brick = new Tile(res.BrickWall, 2);
            rock.IsSolid = true;
            Tile grassstone = new Tile(res.FloorsMedieval, 3);
            rock.IsSolid = false;
            TileRepository.Add(grass);
            TileRepository.Add(rock);
            TileRepository.Add(Brick);
            TileRepository.Add(grassstone);

            game = new Game(this.CreateGraphics(), gw, this.Size);
            this.Size = Game.Size;
            gw.GameObjects.Add(p);
            gw.GameObjects.Add(p2);

            t = new Thread(Run);
            t.Start();
        }

        private void Run()
        {
            while (true)
            {
                try
                {
                    var watch = Stopwatch.StartNew();
                    watch.Stop();
                    p.Angle = (int)CalcAngle(new Point(Game.Size.Width / 2, Game.Size.Height / 2), dest);
                    foreach (var o in gw.VisibleGameObjects)
                        o.MotionBehavior.Move();

                    gw.CheckCollisions();

                    game.Render();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                //Thread.Sleep(1000 / 60);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Fireball f = new Fireball(res.Fireball, gw, p);
                p.Attack(f);
                IGameObject target = gw.GetClosestGameObject(dest, null, 300);

                if (target != null)
                    f.MotionBehavior = new OffensiveMotionBehavior(f, gw, target.Location);
                else
                    f.MotionBehavior = new NormalMotionBehavior(f, gw);

                f.MotionBehavior.CanMoveThroughObjects = true;
            }
            else
                Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Abort();
        }

        private static double CalcAngle(Point first, Point second)
        {
            float dx = second.X - first.X;
            float dy = second.Y - first.Y;
            return Math.Atan2(dy, dx) * 180 / Math.PI;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            p.TargetSpeed = 6;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            p.TargetSpeed = 0;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                dest = new Point(e.X - gw.Player.Image.Size.Width/2, e.Y - gw.Player.Image.Size.Height/2);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
