using System.Drawing;

namespace SpaceInvadersGame.Entity
{
  public class PlayerBullet : BulletBase
  {
    protected override int Speed => -3;

    public override void Render(Graphics g)
    {
      g.FillRectangle(Style.PlayerBulletBrush, new Rectangle((int)this.X,
        (int)this.Y, this.Width, this.Height));
    }

    protected override bool HasShotEntity(EntityBase entity)
    {
      var isPlayer = entity is Player;
      return base.HasShotEntity(entity) && !isPlayer;
    }

    public PlayerBullet(double x, double y)
      : base(x, y)
    { }
  }
}
