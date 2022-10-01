using System;
using System.Collections.Generic;
using System.Drawing;
using SpaceInvadersGame.Entity;

namespace SpaceInvadersGame.Screens
{
  public class GameScreen : GScreen, IEntityContainer
  {
    private const int paddingTop = 25;
    private const int paddingRight = 5;
    private const int paddingLeft = 5;
    private const int paddingBottom = 5;

    private readonly List<EntityBase> entities;
    private EnemySquadron enemySquad;
    private Player player;
    private int playerLives = 3;
    private EntityBase lastShip;
    private int waitShipTicks = 0;
    private readonly Bitmap screen;
    private int score = 0;
    private int gameOverScreenWaitTicks = -1;
    private int winScreenWaitTicks = -1;

    public override void Render(Graphics g)
    {
      using(var sg = Game.GetGraphics(this.screen))
      {
        sg.FillRectangle(Brushes.Black, 
          new Rectangle(0, 0, this.screen.Width, this.screen.Height));
        foreach (var entity in this.entities)
          entity.Render(sg);
      }

      g.DrawImage(this.screen, paddingLeft, paddingTop);
      g.DrawRectangle(Style.BorderPen, new Rectangle(paddingLeft, paddingTop,
        Game.WIDTH - paddingLeft - paddingRight, Game.HEIGHT - paddingTop - paddingBottom));
      DrawString("score:", g, 5, 11);
      DrawString(this.score.ToString("000000"), g, 45, 11);
      DrawString("lives:", g, 200, 11);
      Bitmap playerArt = Art.Player[0, 0];
      for(int i = 0; i < this.playerLives - 1; i++)
      {
        g.DrawImage(playerArt, 240 + (playerArt.Width + 5) * i, 8);
      }
    }

    public override void Tick(Input input)
    {
      if (this.WaitGameOverScreen() || this.WaitWinScreen())
        return;

      if (this.enemySquad.Height + this.enemySquad.Y >= this.player.Y)
        this.SetScreen(new GameOverScreen());

      this.player.Tick(input);
      if(this.lastShip == null || this.lastShip.Removed)
      {
        if(--this.waitShipTicks == 0)
        {
          var motherShip = new MotherShip(this.screen.Width, 8);
          motherShip.ShipDied += this.EnemyShipDied;
          this.lastShip = motherShip;
          this.AddEntity(this.lastShip);
          this.waitShipTicks = EntityBase.Random.Next(10 * 60, 20 * 60);
        }
      }

      for (int i = 0; i < this.entities.Count; i++)
      {
        var entity = this.entities[i];
        if (entity.Removed)
        {
          this.entities.RemoveAt(i--);
          continue;
        }

        entity.Tick();
      }
    }

    private void Initialize()
    {
      this.InitPlayer();

      this.enemySquad = new EnemySquadron(0, 32);
      this.enemySquad.ShipDied += this.EnemyShipDied;
      this.enemySquad.SquadronDied += this.SquadDied;
      this.AddEntity(this.enemySquad);

      this.InitShield(55, 225);
      this.InitShield(125, 225);
      this.InitShield(195, 225);
    }

    private void InitPlayer()
    {
      this.player = new Player(screen.Width / 2, screen.Height);
      this.player.PlayerShoted += this.PlayerShooted;
      this.AddEntity(this.player);
    }

    private void InitShield(double xs, double ys)
    {
      var leftTopShield = new ShieldPart(xs, ys, EShieldPart.LeftTop);
      this.AddEntity(leftTopShield);

      var middleLeftTopShield = new ShieldPart(leftTopShield.X + leftTopShield.Width,
        leftTopShield.Y, EShieldPart.Middle);
      this.AddEntity(middleLeftTopShield);

      var middleRightTopShield = new ShieldPart(middleLeftTopShield.X + middleLeftTopShield.Width,
        middleLeftTopShield.Y, EShieldPart.Middle);
      this.AddEntity(middleRightTopShield);

      var rightTopShield = new ShieldPart(middleRightTopShield.X + middleRightTopShield.Width,
        middleRightTopShield.Y, EShieldPart.RightTop);
      this.AddEntity(rightTopShield);

      var leftBottomShield = new ShieldPart(leftTopShield.X,
        leftTopShield.Y + leftTopShield.Height, EShieldPart.Middle);
      this.AddEntity(leftBottomShield);

      var middleLeftBottomShield = new ShieldPart(middleLeftTopShield.X,
        middleLeftTopShield.Y + middleLeftTopShield.Height, EShieldPart.LeftBottom);
      this.AddEntity(middleLeftBottomShield);

      var middleRightBottomShield = new ShieldPart(middleRightTopShield.X,
        middleRightTopShield.Y + middleRightTopShield.Height, EShieldPart.RightBottom);
      this.AddEntity(middleRightBottomShield);

      var bottomShieldLeft = new ShieldPart(leftBottomShield.X,
        leftBottomShield.Y + leftBottomShield.Height, EShieldPart.Middle);
      this.AddEntity(bottomShieldLeft);

      var bottomShieldRight = new ShieldPart(rightTopShield.X,
        rightTopShield.Y + rightTopShield.Height, EShieldPart.Middle);
      this.AddEntity(bottomShieldRight);

      var rightBottomShield = new ShieldPart(bottomShieldRight.X,
        bottomShieldRight.Y + bottomShieldRight.Height, EShieldPart.Middle);
      this.AddEntity(rightBottomShield);
    }


    private void EnemyShipDied(object sender, DieShipEventArgs args)
    {
      switch (args.DiedEnemyType)
      {
        case EEnemyType.Front:
          this.score += 10;
          break;
        case EEnemyType.Middle:
          this.score += 20;
          break;
        case EEnemyType.Back:
          this.score += 30;
          break;
        case EEnemyType.MotherShip:
          this.score += EntityBase.Random.Next(40, 100);
          break;
      }
    }

    private void SquadDied(object sender, DieSquadEventArgs args)
    {
      this.winScreenWaitTicks = 60;
    }

    private void PlayerShooted(object sender, PlayerShotedArgs args)
    {
      this.player.PlayerShoted -= this.PlayerShooted;
      if (--this.playerLives > 0)
        this.InitPlayer();
      else
        this.gameOverScreenWaitTicks = 60;
    }

    private bool WaitGameOverScreen()
    {
      if (this.gameOverScreenWaitTicks == -1)
        return false;
      if (--this.gameOverScreenWaitTicks == 0)
        this.SetScreen(new GameOverScreen());

      return true;
    }

    private bool WaitWinScreen()
    {
      if (this.winScreenWaitTicks == -1)
        return false;
      if (--this.winScreenWaitTicks == 0)
        this.SetScreen(new WinScreen(this.score));

      return true;
    }

    #region IEntityContainer

    public IEnumerable<EntityBase> Entities => entities;

    public void AddEntity(EntityBase entity)
    {
      this.entities.Add(entity);
      entity.Init(this);
    }

    public bool IsFree(double x, double y, int width, int height)
    {
      if (x < 0 || this.screen.Width - width < x)
        return false;

      if (y < 0 || this.screen.Height < y + height)
        return false;

      return true;
    }

    #endregion

    public GameScreen()
    {
      this.entities = new List<EntityBase>();
      this.screen = new Bitmap(Game.WIDTH - paddingLeft - paddingRight,
        Game.HEIGHT - paddingTop - paddingBottom);
      this.waitShipTicks = EntityBase.Random.Next(5 * 60, 10 * 60);
      this.Initialize();
    }
  }
}
