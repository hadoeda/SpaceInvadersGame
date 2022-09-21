using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvadersGame.Screens
{
  public abstract class GScreen
  {
    private IScreenContainer container;

    private static readonly string[] chars = new[] {
      "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
      ".,!?:;\"'+-=/\\<"
    };
    
    public void Init(IScreenContainer container)
    {
      this.container = container;
    }

    public abstract void Render(Graphics g);

    public abstract void Tick(Input input);

    public static void DrawString(string str, Graphics g, int x, int y)
    {
      str = str.ToUpper();
      for(int i = 0; i < str.Length; i++)
      {
        char ch = str[i];
        for(int ys = 0; ys < chars.Length; ys++)
        {
          int xs = chars[ys].IndexOf(ch);
          if(xs >= 0)
          {
            g.DrawImage(Art.Letters[xs, ys], x + i * 6, y);
          }
        }
      }
    }

    protected void SetScreen(GScreen screen)
    {
      this.container.SetScreen(screen);
    }
  }
}
