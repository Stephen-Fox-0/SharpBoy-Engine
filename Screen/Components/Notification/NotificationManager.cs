using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Components.Notification
{
    public class NotificationSystem : SceneSystemComponent
    {
        Game _game;
        Anchor _anchor;
        float _timeScale;
        bool _enabled = true;

        public bool AudioEnabled { get; set; }
        public float AudioVolume { get; set; }

        /// <summary>
        /// Initialize a new instance of <see cref="NotificationManager"/>
        /// </summary>
        /// <param name="game">The game we are attached to.</param>
        public NotificationSystem( Game game ):base(game)
        {
            _game = game;
        }

        /// <summary>
        /// Gets or sets the anchor of our notifications.
        /// </summary>
        public Anchor Anchor
        {
            get => _anchor;
            set => _anchor = value;
        }

        /// <summary>
        /// Gets or sets the timescale of how long the notification is shown.
        /// </summary>
        public float TimeScale
        {
            get => _timeScale;
            set => _timeScale = value;
        }

        /// <summary>
        /// Gets or set weather notifications are enabled.
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }


        public void Show( string message, Icon icon, int timeOnScreen)
        {
            if (Enabled)
            {
                var notify = new Notification(message, _game, icon, Anchor, TimeScale, timeOnScreen);
                notify.AudioEnabled = AudioEnabled;
                notify.AudioVolume = AudioVolume;
                _game.Components.Add(notify);
            }
        }
        public void Show( string message, Icon icon )
        {
            Show( message, icon, 3000 );
        }
        public void Show(string message)
        {
            Show( message, Icon.Normal, 3000 );
        }
    }
}
