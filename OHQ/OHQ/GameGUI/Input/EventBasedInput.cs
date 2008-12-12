using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace OHQ.GameGUI.Input
{
    #region Delegates

    public delegate void OnMouseButtonDownEventHandler(object sender, MouseEventArgs e);
    public delegate void OnMouseButtonUpEventHandler(object sender, MouseEventArgs e);

    public delegate void OnKBKeyDownEventHandler(object sender, KeyboardEventArgs e);
    public delegate void OnKBKeyUpEventHandler(object sender, KeyboardEventArgs e);


    #endregion

    public interface IEventBasedInput
    {

    }
}