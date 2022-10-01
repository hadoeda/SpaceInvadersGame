using System.Drawing;

namespace SpaceInvadersGame
{
  public static class Style
  {
    public readonly static Color PlayerColor = Color.FromArgb(128, 128, 128);
    public readonly static Brush PlayerBrush = new SolidBrush(PlayerColor);
    public readonly static Color BorderColor = Color.FromArgb(128, 128, 128);
    public readonly static Brush BorderBrush = new SolidBrush(BorderColor);
    public readonly static Pen BorderPen = new Pen(BorderBrush, 1);
    public readonly static Color TextColor = Color.FromArgb(128, 128, 128);
    public readonly static Brush TextBrush = new SolidBrush(TextColor);
    public readonly static Font Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
    public readonly static Brush EnemyFrontBrush = new SolidBrush(Color.FromArgb(0, 255, 255));
    public readonly static Brush EnemyMiddleBrush = new SolidBrush(Color.FromArgb(255, 216, 0));
    public readonly static Brush EnemyBackBrush = new SolidBrush(Color.FromArgb(178, 0, 255));
    public readonly static Brush PlayerBulletBrush = new SolidBrush(PlayerColor);
    public readonly static Brush EnemyBulletBrush = new SolidBrush(Color.Red);
  }
}
