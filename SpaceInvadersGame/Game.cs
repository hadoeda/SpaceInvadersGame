using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using SpaceInvadersGame.Screens;

namespace SpaceInvadersGame
{
  public class Game : Form, IScreenContainer
  {
    public const int WIDTH = 280;
    public const int HEIGHT = 300;
    public const int SCREEN_SCALE = 2;
    public const int FPS = 60;
    public const string NAME = "Space Invaders";

    private GScreen screen;
    private Input input;
    private bool running;
    private Thread workTread;

    protected override void OnClosed(EventArgs e)
    {
      running = false;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      this.input.SetKey(e.KeyCode, true);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
      this.input.SetKey(e.KeyCode, false);
    }

    private void Initialize()
    {
      this.ClientSize = new Size(WIDTH * SCREEN_SCALE, HEIGHT * SCREEN_SCALE);
      this.Text = NAME;
      this.Click += GameClick;

      this.input = new Input();
      this.running = true;
      this.workTread = new Thread(Work)
      {
        IsBackground = true
      };
      this.workTread.Start();
    }

    private void GameClick(object sender, EventArgs e)
    {
      if (screen is TitleScreen) this.SetScreen(new GameScreen());
    }

    private void Work()
    {
      var image = new Bitmap(WIDTH, HEIGHT);
      this.SetScreen(new TitleScreen());


      var timer = Stopwatch.StartNew();
      long elapsed = 0;
      Thread.Sleep(100);
      while (running)
      {
        elapsed += timer.ElapsedMilliseconds;
        timer.Restart();

        int max = 10;
        while (elapsed > 0)
        {
          elapsed -= 1000 / FPS;
          screen.Tick(input);
          input.Tick();
          if (max-- == 0)
          {
            elapsed = 0;
            break;
          }
        }

        using (var g = GetGraphics(image))
        {
          g.FillRectangle(Brushes.Black, new Rectangle(0, 0, WIDTH, HEIGHT));
          this.screen.Render(g);
          using (var wg = GetGraphics())
          {
            wg.DrawImage(image, 0, 0, WIDTH * SCREEN_SCALE, HEIGHT * SCREEN_SCALE);
          }
        }

        Thread.Sleep(1);
      }
    }

    private Graphics GetGraphics()
    {
      var g = this.CreateGraphics();
      g.InterpolationMode = InterpolationMode.NearestNeighbor;
      g.CompositingQuality = CompositingQuality.AssumeLinear;
      g.SmoothingMode = SmoothingMode.None;
      return g;
    }

    public static Graphics GetGraphics(Image image)
    {
      var g = Graphics.FromImage(image);
      g.InterpolationMode = InterpolationMode.NearestNeighbor;
      g.CompositingQuality = CompositingQuality.AssumeLinear;
      g.SmoothingMode = SmoothingMode.None;
      return g;
    }

    #region IScreenContainer

    public void SetScreen(GScreen screen)
    {
      this.screen = screen;
      this.screen.Init(this);
    }

    #endregion

    public Game()
    {
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Icon = Icon.FromHandle(Resources.icon.GetHicon());
      this.Initialize();
    }

    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Game());
    }
  }
}
