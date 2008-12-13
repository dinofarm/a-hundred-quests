using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OHQ.GameGUI.Input
{
    public enum Player
    {
        One = 1,
        Two = 2,
        Three = 4,
        Four = 8
    }
    public enum GamepadButton
    {
        A, B, X, Y,
        RightShoulder, LeftShoulder,
        RightStick, LeftStick,
        LeftStickUp, LeftStickDown,
        LeftStickLeft, LeftStickRight,
        RightStickUp, RightStickDown,
        RightStickLeft, RightStickRight,
        LeftTrigger, RightTrigger,
        DpadUp, DpadDown, DpadLeft, DpadRight,
        Back, Start, BigButton
    }
    public enum MouseButton
    {
        Left, Right, Middle, Button4, Button5
    }
    public class CancelEventArgs : System.EventArgs
    {
        #region Properties
        public bool Canceled { get; set; }
        #endregion
    }
    public class GamepadEventArgs : System.EventArgs
    {
        #region Properties
        public GamePadState State { get; set; }
        public GamepadButton Button { get; set; }
        public GamePadDPad DPad { get; set; }
        
        private PlayerIndex m_PlayerIndex = PlayerIndex.One;
        public PlayerIndex PlayerIndex
        {
            get { return m_PlayerIndex; }
            set { m_PlayerIndex = value; }
        }
        #endregion
    }
    public class MouseEventArgs : System.EventArgs
    {
        #region Properties

        public MouseState State { get; set; }
        public MouseButton MouseButton { get; set; }
        public Point Posistion { get; set; }

        #endregion
    }

    public class KeyboardEventArgs : System.EventArgs
    {
        #region Properties
        public Keys Key { get; set; }
        public bool Alt { get; set; }
        public bool Ctrl { get; set; }
        public bool Shift { get; set; }
        #endregion
    }
}
