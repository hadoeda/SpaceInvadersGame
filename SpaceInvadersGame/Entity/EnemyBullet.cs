using System.Drawing;

namespace SpaceInvadersGame.Entity
{
  public class EnemyBullet : BulletBase
  {
    protected override int Speed => 3;

    public override void Render(Graphics g)
    {
      g.FillRectangle(Style.EnemyBulletBrush, new Rectangle((int)this.X,
        (int)this.Y, this.Width, this.Height));
    }

    protected override bool HasShotEntity(EntityBase entity)
    {
      var isEnemy = entity is EnemyBase;
      return base.HasShotEntity(entity) && !isEnemy;
    }


    public EnemyBullet(double x, double y)
      : base(x, y)
    {}
  }
}
