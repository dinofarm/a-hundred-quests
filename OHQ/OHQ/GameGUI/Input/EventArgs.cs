using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OHQ.GameGUI.Input
{
    #region Mouse
    public enum MouseButton
    {
        Left, Right, Middle,
        Button4, Button5
    }

    public class MouseEventArgs : System.EventArgs
    {
        #region Properties

        public MouseState State { get; set; }

        public MouseButton MouseButton { get; set; }

        public Point Posistion { get; set; }

        #endregion
    }

    public delegate void OnMouseButtonDownEventHandler(object sender, MouseEventArgs e);
    public delegate void OnMouseButtonUpEventHandler(object sender, MouseEventArgs e);
    #endregion
}
