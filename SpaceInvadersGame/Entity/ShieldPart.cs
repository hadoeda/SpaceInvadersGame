using System.Drawing;

namespace SpaceInvadersGame.Entity
{
  public class ShieldPart : EntityBase
  {
    private int damage = 0;
    private EShieldPart part;

    private int index => (int)this.part;

    public override void Render(Graphics g)
    {
      g.DrawImage(Art.Shields[this.damage, this.index], (int)this.X, (int)this.Y);
    }

    public override bool Shot(BulletBase bullet)
    {
      var shoted = bullet.X >= this.X && bullet.X <= this.X + this.Width &&
        bullet.Y >= this.Y && bullet.Y <= this.Y + this.Height;
      if (shoted && ++this.damage > 3)
      {
        this.damage = 3;
        this.Remove();
      }

      return shoted;
    }

    public ShieldPart(double x, double y, EShieldPart part)
    {
      this.X = x;
      this.Y = y;
      this.part = part;
      this.Width = Art.Shields[0, this.index].Width;
      this.Height = Art.Shields[0, this.index].Height;
    }
  }
}
