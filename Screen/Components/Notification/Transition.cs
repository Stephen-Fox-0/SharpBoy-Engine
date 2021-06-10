using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components.Notification
{
    public enum Transition
    {

        /// <summary>
        /// Indercates that the current screen is transitioning on the display.
        /// </summary>
        TransitionOn = 4,

        /// <summary>
        /// Indercates that the current screen is transitioning off the display.
        /// </summary>
        TransitionOff = 5,

        /// <summary>
        /// Indercates that the current screen is exiting.
        /// </summary>
        Exiting = 6,

        /// <summary>
        /// Indercates that the current screen is active.
        /// </summary>
        Active = 7,

        /// <summary>
        /// Indercates that the current screen is hidden.
        /// </summary>
        Hidden = 8,
    }
}
