using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OHQ.GameGUI.Input
{
    #region Delegates
    /// <summary>
    /// This delegate is called when a mouse button is clicked
    /// </summary>
    /// <param name="e"></param>
    public delegate void MouseButtonClickHandler(MouseEventArgs e);
    /// <summary>
    /// The delegate is called when a mouse button is released
    /// </summary>
    /// <param name="e"></param>
    public delegate void MouseButtonReleaseHandler(MouseEventArgs e);

    /// <summary>
    /// This delegate is called when a Keyboard key is pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void KBKeyPressHandler(object sender, KeyboardEventArgs e);
    /// <summary>
    /// This delegate is called when a Keyboard button is released
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void KBKeyReleaseHandler(object sender, KeyboardEventArgs e);

    /// <summary>
    /// This delegate is called when a XBox360 button, joystick, trigger, or shoulder
    /// is pressed on the gamepad
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GamepadButtonReleaseHandler(object sender, GamepadEventArgs e);
    /// <summary>
    /// This delegate is called when a XBox360 button, joystick, trigger, or shoulder
    /// is released on the gamepad
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GamepadButtonPressHandler(object sender, GamepadEventArgs e);

    public delegate void NextTabHandler(CancelEventArgs e);
    public delegate void PrevTabHandler(CancelEventArgs e);

    /// <summary>
    /// This delegate is called when a player connects and XBox360 controller
    /// </summary>
    /// <param name="playerIndex">The index of the player that was connected</param>
    public delegate void PlayerConnectHandler(PlayerIndex playerIndex);
    /// <summary>
    /// This delegate is called when a player disconnects a XBox360 controller
    /// </summary>
    /// <param name="playerIndex">This index of the player that awas disconnected</param>
    public delegate void PlayerDisconnectHandler(PlayerIndex playerIndex);

    /// <summary>
    /// This delegate is a generalization of selecting a menu option and
    /// rather than having a OnKbDownEventDown or Up handler this is a 
    /// generic event that can be used for the keyboard, mouse, or gamepad
    /// </summary>
    /// <param name="sender">The control</param>
    public delegate void MenuSelectHandler(object sender);
    /// <summary>
    /// This delegate is a generalization of go backwards on a menu option and
    /// rather than having a OnKbDownEventDown or Up handler this is a 
    /// generic event that can be used for the keyboard, mouse, or gamepad
    /// </summary>
    /// <param name="sender">the object</param>
    public delegate void MenuBackHandler(object sender);

    #endregion

    /// <summary>
    /// All controller classes should from this class in order to be compliant with the 
    /// event based input system. All virual functions should be overriden in derived classes
    /// especially Update().
    /// </summary>
    public class ControllerBase
    {
        private IEventBasedInputService m_EbiService;
        public IEventBasedInputService EbiService
        {
            get { return m_EbiService; }
            set
            {
                m_EbiService = value;
                OnEbiServiceSet();
            }
        }
        public virtual void Update(GameTime gameTime){}
        protected virtual void OnEbiServiceSet(){}
    }


    public interface IEventBasedInputService
    {
        #region Interface Properties
        bool TabEnabled { get; set; }
        float RepeatInterval { get; set; }
        Player AllowedPlayers { get; set; }
        IFocusable Focusable { get; set; }
        List<Keys> TabKeys { get; set; }
        List<GamepadButton> TabNext { get; set; }
        List<GamepadButton> TabPrev { get; set; }
        #endregion

        #region Interface Events
        event PlayerConnectHandler OnPlayerConnect;
        event PlayerDisconnectHandler OnPlayerDisconnect;
        event MouseButtonClickHandler OnRequestingFocus;
        event MouseButtonClickHandler OnMouseButtonClick;
        event MouseButtonReleaseHandler OnMouseButtonRelease;
        event KBKeyPressHandler OnKeyboardKeyPress;
        event KBKeyReleaseHandler OnKeyboardKeyRelease;
        event GamepadButtonPressHandler OnGamepadButtonPress;
        event GamepadButtonReleaseHandler OnGamepadButtonRelease;
        event MenuSelectHandler OnMenuSelectPressed;
        event MenuSelectHandler OnMenuSelectReleased;
        event MenuBackHandler OnMenuBackPressed;
        event MenuBackHandler OnMenuBackReleased;
        event NextTabHandler NextTab;
        event PrevTabHandler PrevTab;
        #endregion

        #region Interface Methods
        void Select();
        void Back();
        void TabToNext(CancelEventArgs args);
        void TabToPrev(CancelEventArgs args);
        void SimulateKey(KeyboardEventArgs args, bool down);
        void SimulateButton(GamepadEventArgs args, bool down);
        void SimulateMouse(MouseEventArgs args, bool down);
        void ConnectPlayer(PlayerIndex index);
        void DisconnectPlayer(PlayerIndex index);
        void RequestFocus(MouseEventArgs args);
        #endregion
    }

    public sealed class EventBasedInput<T> 
        : GameComponent, IEventBasedInputService where T : ControllerBase, new()
    {
        #region Properties
        public float RepeatInterval
        {
            get { return m_RepeatInterval;} set { m_RepeatInterval = value;}
        }
        public Player AllowedPlayers
        {
            get { return m_AllowedPlayers;} set { m_AllowedPlayers = value;}
        }
        public IFocusable Focusable
        {
            get { return m_Focus;}
            set
            {
                if(value.Focus() && m_Focus != null)
                    m_Focus.UnFocus();
                m_Focus = value;
            }
        }
        public List<Keys> TabKeys
        {
            get { return m_TabKeys; }
            set { m_TabKeys = value; }
        }
        public List<GamepadButton> TabNext
        {
            get{ return m_TabNext;} set { m_TabNext = value;}
        }
        public List<GamepadButton> TabPrev
        {
            get { return m_TabPrev;} set { m_TabPrev = value;}
        }
        public bool TabEnabled
        {
            get { return m_TabEnabled; }
            set { m_TabEnabled = value;}
        }
        #endregion

        #region Data Fields
        private bool m_TabEnabled = true;
        private float m_RepeatInterval = 250.0f;
        private Player m_AllowedPlayers = Player.One;
        private IFocusable m_Focus = null;
        private ControllerBase m_Controller = null;
        private List<Keys> m_TabKeys = new List<Keys>();
        private List<GamepadButton> m_TabNext = new List<GamepadButton>();
        private List<GamepadButton> m_TabPrev = new List<GamepadButton>();
        private List<GamepadButton> m_SelectNext = new List<GamepadButton>();
        private List<GamepadButton> m_SelectPrev = new List<GamepadButton>();
        #endregion

        #region Events
        public event PlayerConnectHandler OnPlayerConnect;
        public event PlayerDisconnectHandler OnPlayerDisconnect;
        public event MouseButtonClickHandler OnRequestingFocus;
        public event MouseButtonClickHandler OnMouseButtonClick;
        public event MouseButtonReleaseHandler OnMouseButtonRelease;
        public event KBKeyPressHandler OnKeyboardKeyPress;
        public event KBKeyReleaseHandler OnKeyboardKeyRelease;
        public event GamepadButtonPressHandler OnGamepadButtonPress;
        public event GamepadButtonReleaseHandler OnGamepadButtonRelease;
        public event MenuSelectHandler OnMenuSelectPressed;
        public event MenuSelectHandler OnMenuSelectReleased;
        public event MenuBackHandler OnMenuBackPressed;
        public event MenuBackHandler OnMenuBackReleased;
        public event NextTabHandler NextTab;
        public event PrevTabHandler PrevTab;
        #endregion

        #region Ctor
        public EventBasedInput(Game game)
            : base(game)
        {
            // Create the controller
            m_Controller = new T();
            m_Controller.EbiService = this;

            // Add the tab keys
            m_TabKeys.Add(Keys.Tab);
            m_TabNext.Add(GamepadButton.LeftStickRight);
            m_TabPrev.Add(GamepadButton.LeftStickLeft);
            m_SelectNext.Add(GamepadButton.A);
            m_SelectPrev.Add(GamepadButton.B);
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            // Only update the controller if we have one
            if (m_Controller != null)
                m_Controller.Update(gameTime);

            base.Update(gameTime);
        }

        public void Select()
        {
            if(OnMenuSelectPressed != null)
                OnMenuSelectPressed.Invoke(m_Focus);
        }

        public void Back()
        {
            if(OnMenuBackPressed != null)
                OnMenuBackPressed.Invoke(m_Focus);
        }

        public void TabToNext(CancelEventArgs cancelEventArgs)
        {
            if (NextTab != null)
                NextTab.Invoke(cancelEventArgs);
        }

        public void TabToPrev(CancelEventArgs cancelEventArgs)
        {
            if (PrevTab != null)
                PrevTab.Invoke(cancelEventArgs);
        }

        public void ConnectPlayer(PlayerIndex playerIndex)
        {
            if(OnPlayerConnect != null)
                OnPlayerConnect.Invoke(playerIndex);
        }

        public void DisconnectPlayer(PlayerIndex playerIndex)
        {
            if(OnPlayerDisconnect != null)
                OnPlayerDisconnect.Invoke(playerIndex);
        }

        public void RequestFocus(MouseEventArgs mouseEventArgs)
        {
           if(OnRequestingFocus != null)
               OnRequestingFocus.Invoke(mouseEventArgs);
        }

        public void SimulateKey(KeyboardEventArgs keyboardEventArgs, bool down)
        {
            if(down && OnKeyboardKeyPress != null)
                OnKeyboardKeyPress.Invoke(m_Focus, keyboardEventArgs);
            else if(!down && OnKeyboardKeyRelease != null)
                OnKeyboardKeyRelease.Invoke(m_Focus, keyboardEventArgs);
        }

        public void SimulateMouse(MouseEventArgs mouseEventArgs, bool down)
        {
            if(down && OnMouseButtonClick != null)
                OnMouseButtonClick.Invoke(mouseEventArgs);
            else if(!down && OnMouseButtonRelease != null)
                OnMouseButtonRelease.Invoke(mouseEventArgs);
        }


        public void SimulateButton(GamepadEventArgs gamepadEventArgs, bool down)
        {
            if(down)
            {

                if(OnGamepadButtonPress != null)
                    OnGamepadButtonPress.Invoke(m_Focus, gamepadEventArgs);
                if(m_SelectNext.Contains(gamepadEventArgs.Button))
                {
                    if (OnMenuSelectPressed != null)
                        OnMenuSelectPressed.Invoke(m_Focus);

                }
                else if(m_SelectPrev.Contains(gamepadEventArgs.Button))
                {
                    if(OnMenuBackPressed != null)
                        OnMenuBackPressed.Invoke(m_Focus);
                }
            }
            if (!down)
            {

                if (OnGamepadButtonRelease != null)
                    OnGamepadButtonRelease.Invoke(m_Focus, gamepadEventArgs);
                if (m_SelectNext.Contains(gamepadEventArgs.Button))
                {
                    if (OnMenuSelectReleased != null)
                        OnMenuSelectReleased.Invoke(m_Focus);

                }
                else if (m_SelectPrev.Contains(gamepadEventArgs.Button))
                {
                    if (OnMenuBackReleased != null)
                        OnMenuBackReleased.Invoke(m_Focus);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// This class is the representation of a player using the mouse and keyboard
    /// </summary>
    public class MouseKBController : ControllerBase
    {
        protected class InputState
        {
            public bool Pressed;
            public float TimePressed;
        }
        
        protected Dictionary<Keys, InputState> m_keys = new Dictionary<Keys, InputState>();
        protected MouseState m_CurrentMouseState;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(EbiService == null)
                return;

            UpdateKeyboard(gameTime);
            UpdateMouse(gameTime);
        }

        protected virtual void UpdateKeyboard(GameTime gameTime)
        {
            KeyboardEventArgs keyboardEventArgs = new KeyboardEventArgs();
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            // This loop cycles through all the new keys pressed by the user
            foreach (Keys key in pressedKeys)
            {
                // If Alt is pressed
                if (key == Keys.LeftAlt || key == Keys.RightAlt)
                    keyboardEventArgs.Alt = true;
                // If Control is pressed
                else if (key == Keys.LeftControl || key == Keys.RightControl)
                    keyboardEventArgs.Ctrl = true;
                // If Shift is pressed
                else if (key == Keys.LeftShift || key == Keys.RightShift)
                    keyboardEventArgs.Shift = true;
                else
                {
                    // If the key wasn't pressed before this update
                    // it is added to our array of keys we're keeping track off
                    if(!m_keys.ContainsKey(key))
                        m_keys[key] = new InputState();
                }
            }

            // This loop loops through all the keys already pressed by the user
            // we take this in two steps because we dont want to prematurely
            // make a user commit to an action
            foreach(Keys key in m_keys.Keys)
            {
                // If the key is Shift, Control, or Alt skip to the next key
                if (key == Keys.LeftControl || key == Keys.RightControl ||
                    key == Keys.LeftShift || key == Keys.RightShift || 
                    key == Keys.LeftAlt || key == Keys.RightAlt)
                    continue;

                // Is key pressed?
                bool keyPressed = keyboardState.IsKeyDown(key);
                
                // Get the keys InputState
                InputState _state = m_keys[key];

                // If the keys was pressed on the previous update add to the total time
                // the key has been pressed
                if (keyPressed)
                    _state.TimePressed += gameTime.ElapsedGameTime.Milliseconds;

                // If the key is pressed now and was not pressed before
                if(keyPressed && !_state.Pressed)
                {
                    // Set the key state to pressed
                    _state.Pressed = true;
                    
                    // Set the event args key
                    keyboardEventArgs.Key = key;

                    // Fire event
                    EbiService.SimulateKey(keyboardEventArgs, true);

                    // Tab keys enabled
                    if(EbiService.TabEnabled)
                    {
                        for(int i = 0; i < EbiService.TabKeys.Count; i++)
                        {
                            if(key == EbiService.TabKeys[i])
                            {
                                if(keyboardEventArgs.Shift)
                                    EbiService.TabToNext(new CancelEventArgs());
                                else
                                    EbiService.TabToPrev(new CancelEventArgs());
                            }
                        }
                    }
                }
            }
            
        }
        protected virtual void UpdateMouse(GameTime gameTime)
        {
            
        }
    }
}