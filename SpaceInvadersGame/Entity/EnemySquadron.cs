using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvadersGame.Entity
{
  public class EnemySquadron : EnemyBase, IEntityContainer
  {
    private const int MovePoints = 4;
    private int tick = 0;
    private int dx = MovePoints;
    private bool wallHitted = false;
    private readonly List<EntityBase> enemies;
    private BulletBase lastBullet;
    private int speed = 2;

    public override int Width => (int)(this.enemies.Max(e => e.X + e.Width) - this.X);
    public override int Height => (int)(this.enemies.Max(e => e.Y + e.Height) - this.Y);

    public IEnumerable<EntityBase> Entities => throw new NotImplementedException();

    public event EventHandler<DieShipEventArgs> ShipDied;
    public event EventHandler<DieSquadEventArgs> SquadronDied;

    public override void Render(Graphics g)
    {
      foreach (var enemy in this.enemies)
        enemy.Render(g);
    }

    public override void Tick()
    {
      for (int i = 0; i < this.enemies.Count; i++)
      {
        var enemy = this.enemies[i];
        if (enemy.Removed)
        {
          this.enemies.RemoveAt(i);
          continue;
        }

        enemy.Tick();
      }

      this.tick++;
      if (this.tick < Game.FPS / speed) return;

      this.X = this.enemies.Min(e => e.X);
      int ya = 0;
      int xa = this.dx;
      if (this.wallHitted)
      {
        ya = 2 * MovePoints;
        xa = 0;
        this.dx = -this.dx;
        this.wallHitted = false;
      }

      this.DoShot();
      this.tick = 0;
      this.TryMove(xa, ya);
    }

    public override void HitWall(double xa, double ya)
    {
      this.wallHitted = true;
    }

    public override bool Shot(BulletBase bullet)
    {
      var diedShip = this.enemies.FirstOrDefault(e => e.Shot(bullet)) as EnemyShip;
      var died = diedShip != null;
      if (died)
      {
        this.ShipDied?.Invoke(this, new DieShipEventArgs(diedShip.Type));
        var enemiesCount = this.enemies.OfType<EnemyShip>().Count();
        if (enemiesCount == 1)
          this.SquadronDied?.Invoke(this, new DieSquadEventArgs());
        if (enemiesCount <= 41 && this.speed < 4)
          this.speed = 4;
        if (enemiesCount <= 21 && this.speed < 6)
          this.speed = 6;
      }

      return died;
    }

    protected override void Move(double xa, double ya)
    {
      foreach (var enemy in this.enemies)
      {
        enemy.X += xa;
        enemy.Y += ya;
      }
    }

    private void DoShot()
    {
      EntityBase shotEnemy;
      do
      {
        var enemyInd = Random.Next(0, this.enemies.Count);
        shotEnemy = this.enemies.ElementAt(enemyInd);
      }
      while (!(shotEnemy is EnemyShip));

      if (this.lastBullet == null || this.lastBullet.Removed)
      {
        var bullet = new EnemyBullet(shotEnemy.X + shotEnemy.Width / 2,
          shotEnemy.Y + shotEnemy.Height);
        this.container.AddEntity(bullet);
        this.lastBullet = bullet;
      }
    }

    private void Initialize()
    {
      double xe = this.X, ye = this.Y;
      this.CreateEnemies((x, y) => new EnemyShip(x, y, EEnemyType.Back), ref xe, ref ye);
      this.CreateEnemies((x, y) => new EnemyShip(x, y, EEnemyType.Middle), ref xe, ref ye);
      this.CreateEnemies((x, y) => new EnemyShip(x, y, EEnemyType.Front), ref xe, ref ye);
    }

    private void CreateEnemies(Func<double, double, EnemyShip> enemyCreator, ref double x0, ref double y0)
    {
      double tx = x0;
      EnemyShip enemy = null;
      for (int i = 0; i < 2; i++)
      {
        for (int j = 0; j < 10; j++)
        {
          enemy = enemyCreator(x0, y0);
          this.AddEntity(enemy);
          x0 += enemy.Width;
        }
        y0 += enemy.Height + MovePoints;
        x0 = tx;
      }
    }


    #region IEntityContainer

    public void AddEntity(EntityBase entity)
    {
      this.enemies.Add(entity);
      entity.Init(this);
    }

    public bool IsFree(double x, double y, int width, int height)
    {
      return false;
    }

    #endregion

    public EnemySquadron(double x, double y)
    {
      this.X = x;
      this.Y = y;
      this.enemies = new List<EntityBase>();
      this.Initialize();
    }
  }

  public class DieShipEventArgs : EventArgs
  {
    public EEnemyType DiedEnemyType { get; }

    public DieShipEventArgs(EEnemyType type)
    {
      this.DiedEnemyType = type;
    }
  }

  public class DieSquadEventArgs : EventArgs
  { }
}
