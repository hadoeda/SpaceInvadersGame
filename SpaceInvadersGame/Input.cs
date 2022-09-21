using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvadersGame
{
  public class Input
  {
    public const int LEFT = 0;
    public const int RIGHT = 1;
    public const int SHOT = 2;
    public const int ESCAPE = 3;
    public const int ANYKEY = 4;

    public bool[] Buttons { get; }
    public bool[] OldButton { get; }

    public void SetKey(Keys key, bool down)
    {
      int button = -1;
      if (key == Keys.Left || key == Keys.A)
        button = LEFT;
      if (key == Keys.Right || key == Keys.D)
        button = RIGHT;
      if (key == Keys.Up || key == Keys.Space || key == Keys.W)
        button = SHOT;
      if (key == Keys.Escape)
        button = ESCAPE;

      if (button >= 0)
        this.Buttons[button] = down;

      this.Buttons[ANYKEY] = down;
    }

    public void Tick()
    {
      for(int i = 0; i < Buttons.Length; i++)
      {
        this.OldButton[i] = this.Buttons[i];
      }
    }

    public Input()
    {
      this.Buttons = new bool[64];
      this.OldButton = new bool[64];
    }
  }
}
