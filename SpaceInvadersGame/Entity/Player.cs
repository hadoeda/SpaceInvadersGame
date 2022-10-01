using System;
using System.Drawing;

namespace SpaceInvadersGame.Entity
{
  public class Player : EntityBase
  {

    private BulletBase lastBullet;
    public event EventHandler<PlayerShotedArgs> PlayerShoted;

    public override void Render(Graphics g)
    {
      g.DrawImage(Art.Player[0, 0], (int)this.X, (int)this.Y);
    }

    public void Tick(Input input)
    {
      double speed = 4.5;
      double xa = 0;
      if (input.Buttons[Input.LEFT])
        xa -= speed;
      if (input.Buttons[Input.RIGHT])
        xa += speed;
      this.TryMove(xa, 0);

      if (input.Buttons[Input.SHOT] && !input.OldButton[Input.SHOT])
      {
        double bx = this.X + this.Width / 2;
        double by = this.Y;
        if (this.lastBullet == null || this.lastBullet.Removed)
        {
          var bullet = new PlayerBullet(bx, by);
          this.container.AddEntity(bullet);
          this.lastBullet = bullet;
        }
      }
    }

    public override bool Shot(BulletBase bullet)
    {
      var rem = bullet.X >= this.X && bullet.X <= this.X + this.Width &&
        bullet.Y >= this.Y && bullet.Y <= this.Y + this.Height;

      if (rem)
        this.Shooted();

      return rem;
    }

    private void Shooted()
    {
      this.Remove();
      this.container.AddEntity(new PlayerWreck(this.X, this.Y));
      this.PlayerShoted?.Invoke(this, new PlayerShotedArgs());
    }

    public Player(int spawnX, int spawnY)
    {
      this.Width = Art.Player[0, 0].Width;
      this.Height = Art.Player[0, 0].Height;
      this.X = spawnX - this.Width / 2;
      this.Y = spawnY - this.Height;
    }
  }

  public class PlayerShotedArgs : EventArgs 
  { }
}
