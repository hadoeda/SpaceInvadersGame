using System;

namespace SpaceInvadersGame.Entity
{
  public class MotherShip : EnemyShip
  {
    public event EventHandler<DieShipEventArgs> ShipDied;

    public override void Tick()
    {
      this.TryMove(-1.5, 0);
      base.Tick();
    }

    public override void HitWall(double xa, double ya)
    {
      if (this.X < 0 && this.X <= -this.Width)
      {
        this.Remove();
        return;
      }

      this.X += xa;
    }

    protected override void Die()
    {
      this.ShipDied?.Invoke(this, new DieShipEventArgs(this.Type));
      base.Die();
    }

    public MotherShip(double x, double y)
      : base(x, y, EEnemyType.MotherShip)
    { }
  }
}
