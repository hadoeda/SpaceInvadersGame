using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvadersGame.Screens
{
  public class WinScreen : GScreen
  {
    private int ticks = 0;
    private int score = 0;

    public override void Render(Graphics g)
    {
      g.DrawImage(Art.Scale(Art.Player[0, 0], 2), 120, 80);
      DrawString("You win...", g, 110, 110);
      DrawString("Your score:", g, 80, 130);
      DrawString(this.score.ToString("000000"), g, 150, 130);
      DrawString("Press any key to start new game...", g, 40, 150);
    }

    public override void Tick(Input input)
    {
      if(this.ticks != 60)
      {
        this.ticks++;
        return;
      }

      if (input.Buttons[Input.ANYKEY])
        this.SetScreen(new GameScreen());
    }

    public WinScreen(int score = 0)
    {
      this.score = score;
    }
  }
}
