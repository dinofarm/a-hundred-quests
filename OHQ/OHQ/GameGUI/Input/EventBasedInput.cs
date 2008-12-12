using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace OHQ.GameGUI.Input
{
    #region Delegates

    public delegate void OnMouseButtonDownEventHandler(MouseEventArgs e);
    public delegate void OnMouseButtonUpEventHandler(MouseEventArgs e);

    public delegate void OnKBKeyDownEventHandler(object sender, KeyboardEventArgs e);
    public delegate void OnKBKeyUpEventHandler(object sender, KeyboardEventArgs e);

    public delegate void OnGamepadUpEventHandler(object sender, GamepadEventArgs e);
    public delegate void OnGamepadDownEventHandler(object sender, GamepadEventArgs e);

    public delegate void NextTabEventHandler(CancelEventArgs e);
    public delegate void PrevTabEventHandler(CancelEventArgs e);

    public delegate void OnPlayerConnectEventHandler(PlayerIndex playerIndex);
    public delegate void OnPlayerDisconnectEventHandler(PlayerIndex playerIndex);

    #endregion

    public interface IEventBasedInput
    {

    }
}