using System.Drawing;

namespace SpaceInvadersGame.Entity
{
  public class PlayerWreck : EntityBase
  {
    private int ticks = 0;

    public override void Render(Graphics g)
    {
      g.DrawImage(Art.Player[1, 0], (int)this.X, (int)this.Y);
    }

    public override void Tick()
    {
      if (++this.ticks == 30) this.Remove();
    }

    public PlayerWreck(double x, double y)
    {
      this.X = x;
      this.Y = y;
    }
  }
}
