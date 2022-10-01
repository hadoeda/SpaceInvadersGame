namespace SpaceInvadersGame.Entity
{
  public abstract class BulletBase : EntityBase
  {
    private const int width = 1;
    private const int height = 5;

    protected abstract int Speed { get; }

    public override void Tick()
    {
      this.TryMove(0, this.Speed);
      foreach (var entity in this.container.Entities)
      {
        if (!this.HasShotEntity(entity))
          continue;

        if (entity.Shot(this))
        {
          this.Remove();
          break;
        }
      }
    }

    public override void HitWall(double xa, double ya)
    {
      this.Remove();
    }

    protected virtual bool HasShotEntity(EntityBase entity)
    {
      return this != entity && !entity.Removed;
    }

    public BulletBase(double x, double y)
    {
      this.X = x - width / 2;
      this.Y = y - height;
      this.Width = width;
      this.Height = height;
    }
  }
}
