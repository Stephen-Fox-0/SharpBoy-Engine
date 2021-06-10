using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using SharpBoyEngine.Screen.Input;
using SharpBoyEngine.Screen.Services.Scene;
using SharpBoyEngine.Screen.System;

namespace SharpBoyEngine.Screen
{
    public abstract class Scene : ISceneServicePrevidor
    {
        List<SceneObject> objects;
        bool _initalized = false;

        SceneServiceManager services;


        public SceneServiceManager Services
        {
            get => services;
        }

        public bool IsPopup { get; protected set; }

        protected TimeSpan TransitionOnTime
        {
            get => _transitionOnTime;
            set => _transitionOnTime = value;
        }
        private TimeSpan _transitionOnTime = TimeSpan.Zero;

        protected TimeSpan TransitionOffTime
        {
            private get => _transitionOffTime;
            set => _transitionOffTime = value;
        }
        private TimeSpan _transitionOffTime = TimeSpan.Zero;

        // Ranges from zero (fully active, no transition)
        // to one (transitioned fully off to nothing)
        protected float TransitionPosition
        {
            get => _transitionPosition;
            set => _transitionPosition = value;
        }
        private float _transitionPosition = 1;

        // Ranges from 1 (fully active, no transition)
        // to 0 (transitioned fully off to nothing)
        public float TransitionAlpha => 1f - TransitionPosition;

        // Gets the current screen transition state.
        public SceneState ScreenState
        {
            get => _screenState;
            protected set => _screenState = value;
        }
        private SceneState _screenState = SceneState.TransitionOn;

        // There are two possible reasons why a screen might be transitioning
        // off. It could be temporarily going away to make room for another
        // screen that is on top of it, or it could be going away for good.
        // This property indicates whether the screen is exiting for real:
        // if set, the screen will automatically remove itself as soon as the
        // transition finishes.
        public bool IsExiting
        {
            get => _isExiting;
            protected internal set => _isExiting = value;
        }
        private bool _isExiting;

        // Checks whether this screen is active and can respond to user input.
        protected bool IsActive => !_otherScreenHasFocus &&
                                   (_screenState == SceneState.TransitionOn ||
                                    _screenState == SceneState.Active);

        private bool _otherScreenHasFocus;

        public SceneSystem ScreenManager
        {
            get => _screenManager;
            internal set => _screenManager = value;
        }
        private SceneSystem _screenManager;

        public Scene()
        {
            this.services = new SceneServiceManager(this);
            this.objects = new List<SceneObject>();
        }

        // Gets the gestures the screen is interested in. Screens should be as specific
        // as possible with gestures to increase the accuracy of the gesture engine.
        // For example, most menus only need Tap or perhaps Tap and VerticalDrag to operate.
        // These gestures are handled by the ScreenManager when screens change and
        // all gestures are placed in the InputState passed to the HandleInput method.
        public GestureType EnabledGestures
        {
            get => _enabledGestures;
            protected set
            {
                _enabledGestures = value;

                // the screen manager handles this during screen changes, but
                // if this screen is active and the gesture types are changing,
                // we have to update the TouchPanel ourselves.
                if (ScreenState == SceneState.Active)
                {
                    TouchPanel.EnabledGestures = value;
                }
            }
        }
        private GestureType _enabledGestures = GestureType.None;

        // Gets whether or not this screen is serializable. If this is true,
        // the screen will be recorded into the screen manager's state and
        // its Serialize and Deserialize methods will be called as appropriate.
        // If this is false, the screen will be ignored during serialization.
        // By default, all screens are assumed to be serializable.
        public bool IsSerializable
        {
            get => _isSerializable;
            protected set => _isSerializable = value;
        }
        private bool _isSerializable = true;
        
        // Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        // from being paused or tombstoned.
        // instancePreserved is true if the game was preserved during deactivation, false if the screen is
        // just being added or if the game was tombstoned. On Xbox and Windows this will always be false.
        public virtual void Activate(bool instancePreserved)
        {
            for (int i = 0; i < objects.Count; i++)
                objects[i].LoadContent(ScreenManager.Game.Content);

            _initalized = true;
        }


        // Deactivates the screen. Called when the game is being deactivated due to pausing or tombstoning.
        protected virtual void Deactivate() { }

        // Unload content for the screen. Called when the screen is removed from the screen manager.
        public virtual void Unload() { }

        // Unlike HandleInput, this method is called regardless of whether the screen
        // is active, hidden, or in the middle of a transition.
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;

            if (_isExiting)
            {
                // If the screen is going away to die, it should transition off.
                _screenState = SceneState.TransitionOff;

                if (!UpdateTransitionPosition(gameTime, _transitionOffTime, 1))
                    ScreenManager.RemoveScreen(this);    // When the transition finishes, remove the screen
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                _screenState = UpdateTransitionPosition(gameTime, _transitionOffTime, 1)
                    ? SceneState.TransitionOff
                    : SceneState.Hidden;
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                _screenState = UpdateTransitionPosition(gameTime, _transitionOnTime, -1) 
                    ? SceneState.TransitionOn 
                    : SceneState.Active;
            }

            for (int i = 0; i < objects.Count; i++)
                objects[i].Update(gameTime);

            services.UpdateServices(gameTime);
        }

        private bool UpdateTransitionPosition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;    // How much should we move by?

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            _transitionPosition += transitionDelta * direction;    // Update the transition position

            // Did we reach the end of the transition?
            if (direction < 0 && _transitionPosition <= 0 || direction > 0 && _transitionPosition >= 1)
            {
                _transitionPosition = MathHelper.Clamp(_transitionPosition, 0, 1);
                return false;
            }

            return true;    // Otherwise we are still busy transitioning
        }

        // Unlike Update, this method is only called when the screen is active,
        // and not when some other screen has taken the focus.
        public virtual void HandleInput(GameTime gameTime, InputSystem input)
        {
            for (int i = 0; i < objects.Count; i++)
                objects[i].HandleInput(input);
        }
        public virtual void Draw(GameTime gameTime)
        {
            for (int i = 0; i < objects.Count; i++)
                objects[i].Draw(gameTime, ScreenManager.SpriteBatch);
        }

        // Unlike ScreenManager.RemoveScreen, which instantly kills the screen, this method respects
        // the transition timings and will give the screen a chance to gradually transition off.
        public virtual void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
                ScreenManager.RemoveScreen(this);    // If the screen has a zero transition time, remove it immediately
            else
                _isExiting = true;    // Otherwise flag that it should transition off and then exit.
        }


        public void AddObject<T>(T screenObject) where T : SceneObject
        {
            if (_initalized)
                screenObject.LoadContent(ScreenManager.Game.Content);

            objects.Add(screenObject);
        }

        public void RemoveObject(SceneObject obj)
        {
            objects.Remove(obj);
        }
    }
}