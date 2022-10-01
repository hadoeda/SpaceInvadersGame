using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvadersGame.Screens
{
  public class GameOverScreen : GScreen
  {
    private int ticks = 0;

    public override void Render(Graphics g)
    {
      g.DrawImage(Art.Scale(Art.Player[1, 0], 2), 120, 80);
      DrawString("Game over...", g, 104, 110);
      DrawString("Press any key for try again...", g, 50, 130);
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
  }
}
