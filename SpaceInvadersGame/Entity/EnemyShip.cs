using System;
using System.Drawing;

namespace SpaceInvadersGame.Entity
{
  public class EnemyShip : EnemyBase
  {
    public EEnemyType Type => this.type;
    private int SpriteBankIndex => (int) this.type;
    private int index = 0;
    private int tick = 0;
    private EEnemyType type;

    public override void Render(Graphics g)
    {
      g.DrawImage(Art.Enemies[this.index, this.SpriteBankIndex], 
        (int)this.X, (int)this.Y);
    }

    public override void Tick()
    {
      this.tick++;
      if (this.tick == 60)
      {
        this.index = this.index == 0 ? 1 : 0;
        this.tick = 0;
      }
    }

    public override bool Shot(BulletBase bullet)
    {
      var shoted = bullet.X >= this.X && bullet.X <= this.X + this.Width &&
        bullet.Y >= this.Y && bullet.Y <= this.Y + this.Height;
      if (shoted) 
        this.Die();

      return shoted;
    }

    protected virtual void Die()
    {
      this.Remove();
      this.container.AddEntity(new Spark(this.X, this.Y, this.SpriteBankIndex));
    }

    public EnemyShip(double x, double y, EEnemyType type)
    {
      this.X = x;
      this.Y = y;
      this.type = type;
      this.Width = Art.Enemies[0, this.SpriteBankIndex].Width;
      this.Height = Art.Enemies[0, this.SpriteBankIndex].Height;
    }
  }
}
