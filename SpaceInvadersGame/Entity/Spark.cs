using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvadersGame.Entity
{
  public class Spark : EntityBase
  {
    private readonly int spriteBank = 0;
    private int ticks = 0;

    public override void Render(Graphics g)
    {
      g.DrawImage(Art.Enemies[2, this.spriteBank], 
        (int)this.X, (int)this.Y);
    }

    public override void Tick()
    {
      if (++this.ticks == 30) this.Remove();
    }

    public Spark(double x, double y, int spriteBank)
    {
      this.X = x;
      this.Y = y;
      this.spriteBank = spriteBank;
    }
  }
}
