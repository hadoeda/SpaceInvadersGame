using System.Drawing;

namespace SpaceInvadersGame.Screens
{
  public class TitleScreen : GScreen
  {
    private const char space = ' ';
    private readonly string[] pointTitles = new[] {
      string.Empty,
      string.Empty,
      string.Empty,
      string.Empty
    };
    private int frame = 0;
    private int ticks = 0;
    private static readonly string[] titles = new[] {
      " = ? Mystery",
      " = 30 points",
      " = 20 points",
      " = 10 points"
    };

    public override void Render(Graphics g)
    {
      DrawString("Space Invaders", g, 96, 70);
      DrawString("Press any key to start...", g, 70, 85);
      DrawString("= Score advance table =", g, 70, 130);
      g.DrawImage(Art.Enemies[0, 1], 95, 150);
      DrawString(pointTitles[0], g, 110, 152);
      g.DrawImage(Art.Enemies[0, 2], 95, 170);
      DrawString(pointTitles[1], g, 110, 172);
      g.DrawImage(Art.Enemies[0, 3], 95, 190);
      DrawString(pointTitles[2], g, 110, 192);
      g.DrawImage(Art.Enemies[0, 0], 95, 210);
      DrawString(pointTitles[3], g, 110, 212);
    }

    public override void Tick(Input input)
    {
      if (input.Buttons[Input.ANYKEY])
        this.SetScreen(new GameScreen());
      if (frame == 48) return;

      if (ticks == 10)
      {
        ticks = 0;
        int titleIndex = frame / 12;
        string title = titles[titleIndex];
        int leterIndex = frame % 12;
        if (leterIndex < title.Length)
        {
          char letter = title[leterIndex];
          this.pointTitles[titleIndex] += letter;
          if (letter == space)
          {
            letter = title[leterIndex + 1];
            this.pointTitles[titleIndex] += letter;
            frame++;
          }
        }

        frame++;
      }

      ticks++;
    }

  }
}
