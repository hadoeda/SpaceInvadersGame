using System;
using System.Drawing;

namespace SpaceInvadersGame.Entity
{
  public abstract class EntityBase
  {
    public virtual double X { get; set; }
    public virtual double Y { get; set; }

    public virtual int Width { get; protected set; }
    public virtual int Height { get; protected set; }

    public static readonly Random Random = new Random();
    protected IEntityContainer container;

    public virtual void Init(IEntityContainer container)
    {
      this.container = container;
    }

    public bool Removed { get; private set; }

    public void TryMove(double xa, double ya)
    {
      if (!this.container.IsFree(this.X + xa, this.Y, this.Width, this.Height) ||
        !this.container.IsFree(this.X, this.Y + ya, this.Width, this.Height))
        this.HitWall(xa, ya);
      else
        this.Move(xa, ya);
    }


    public abstract void Render(Graphics g);

    public virtual bool Shot(BulletBase bullet)
    {
      return false;
    }

    public virtual void Tick()
    { }

    public virtual void HitWall(double xa, double ya)
    { }

    public void Remove()
    {
      this.Removed = true;
    }

    protected virtual void Move(double xa, double ya)
    {
      this.X += xa;
      this.Y += ya;
    }
  }
}
