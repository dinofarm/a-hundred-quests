using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OHQ.GameGUI
{
    public delegate void OnFocusEventHandler(object sender);
    public delegate void OnLostFocusEventHandler(object sender);

    public interface IFocusable
    {
        #region Methods
        /// <summary>
        /// Set focus on the object
        /// </summary>
        /// <returns>true if the object received focus and false otherwise</returns>
        bool Focus();

        /// <summary>
        /// Removes focus from the object
        /// </summary>
        void UnFocus();

        bool TabNext();
        bool TabPrev();
        #endregion

        #region Events
        /// <summary>
        /// Called when the object recieves focus
        /// </summary>
        event OnFocusEventHandler OnFocus;

        /// <summary>
        /// Called when the object loses focus
        /// </summary>
        event OnLostFocusEventHandler OnLostFocus;
        #endregion

        #region TabProperties

        /// <summary>
        /// Whether the object be tabbed to
        /// </summary>
        bool IsTabbable { get; set; }

        /// <summary>
        /// Gets or sets the tab order of the object
        /// </summary>
        bool TabOrder { get; set; }
        #endregion
    }
}
