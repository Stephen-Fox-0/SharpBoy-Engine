using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SharpBoyEngine.Graphics;
using SharpBoyEngine.Screen.Components;
using SharpBoyEngine.Screen.Graphics;
using SharpBoyEngine.Screen.Input;
using SharpBoyEngine.Screen.Services;

namespace SharpBoyEngine.Screen.System
{
    // The screen manager is a component which manages one or more GameScreen
    // instances. It maintains a stack of screens, calls their Update and Draw
    // methods at the appropriate times, and automatically routes input to the
    // topmost active screen.
    public class SceneSystem : DrawableGameComponent
    {

        public static SceneSystem Instance;
        public static bool CreateSceneAsync = false;

        public bool inputFrozen = false;

        public readonly SceneGraphicsDevice _graphicsDevice;

        private readonly List<Scene> _screens = new List<Scene>();
        private readonly List<Scene> _tempScreensList = new List<Scene>();
        private readonly List<SceneSystemComponent> components = new List<SceneSystemComponent>();
        private readonly ServiceManager _services;

        private readonly InputSystem _input;

        private SpriteBatch _spriteBatch;
        private Texture2D _blankTexture;

        private bool _isInitialized;
        private bool _traceEnabled;

        public Texture2D BackgroundTexture { get; set; }

        public ServiceManager Services => _services;

        public InputSystem InputSystem
        {
            get => _input;
        }

        // A default SpriteBatch shared by all the screens. This saves
        // each screen having to bother creating their own local instance.
        public SpriteBatch SpriteBatch => _spriteBatch;

        public SceneGraphicsDevice GraphicsDeviceManager => _graphicsDevice;

        // If true, the manager prints out a list of all the screens
        // each time it is updated. This can be useful for making sure
        // everything is being added and removed at the right times.
        public bool TraceEnabled
        {
            get => _traceEnabled;
            set => _traceEnabled = value;
        }

        // Gets a blank texture that can be used by the screens.
        public Texture2D BlankTexture => _blankTexture;


        private Fonting.FontManager _fontManager;
        public Fonting.FontManager FontManager
        {
            get => _fontManager;
        }

        private TextureManager _textureManager;
        public TextureManager TextureManager
        {
            get => _textureManager;
        }
        public SceneSystem(Game game) : base(game)
        {
            // we must set EnabledGestures before we can query for them, but
            // we don't assume the game wants to read them.
            TouchPanel.EnabledGestures = GestureType.None;
            _input = new InputSystem();
            var graphicsDevice = new GraphicsDeviceManager(game);
            _graphicsDevice = new SceneGraphicsDevice(graphicsDevice);
            _services = new ServiceManager(game);
            _services.AddService(_input);
            AddComponent<ServiceManager>(Services);
            Instance = this;

            _fontManager = new Fonting.FontManager();
            _textureManager = new TextureManager(game);
        }

        public override void Initialize()
        {
            base.Initialize();
            _isInitialized = true;
        }

        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            var content = Game.Content;

            foreach (var component in components)
                component.LoadContent(Game.Content);

            _fontManager.Initialize(Game.Content);
            _textureManager.LoadAll();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            _blankTexture.SetData<Color>(new Color[] { Color.White });

            // Tell each of the screens to load their content.
            foreach (var screen in _screens)
            {
                screen.Activate(false);
            }
        }

        protected override void UnloadContent()
        {
            foreach (var screen in _screens)
            {
                screen.Unload();
            }
        }

        // Allows each screen to run logic.
        public override void Update(GameTime gameTime)
        {
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            _tempScreensList.Clear();

            foreach (var screen in _screens)
                _tempScreensList.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (_tempScreensList.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                var screen = _tempScreensList[_tempScreensList.Count - 1];

                _tempScreensList.RemoveAt(_tempScreensList.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == SceneState.TransitionOn || screen.ScreenState == SceneState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus & !inputFrozen)
                    {
                        screen.HandleInput(gameTime, _input);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            if (_traceEnabled)
                TraceScreens();

            for (int i = 0; i < components.Count; i++)
                components[i].Update(gameTime);
        }

        private void TraceScreens()
        {
            var screenNames = new List<string>();

            foreach (var screen in _screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        public override void Draw(GameTime gameTime)
        {
            if (BackgroundTexture != null)
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);
                SpriteBatch.Draw(BackgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                SpriteBatch.End();
            }

            for (int i = 0; i < components.Count; i++)
                components[i].Draw(SpriteBatch, gameTime);

            for(int i = 0; i < _screens.Count;i++)
            {
                var screen = _screens[i];
                if (screen.ScreenState == SceneState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(Scene screen)
        {
            if (CreateSceneAsync)
            {
                Async.AsyncThreadManager.Start(new Async.AsyncThreadData(() =>
                {
                    screen.ScreenManager = this;
                    screen.IsExiting = false;

                    // If we have a graphics device, tell the screen to load content.
                    if (_isInitialized)
                        screen.Activate(false);

                }, () =>
                {
                    _screens.Add(screen);
                    TouchPanel.EnabledGestures = screen.EnabledGestures;
                }));
            }
            else
            {
                screen.ScreenManager = this;
                screen.IsExiting = false;

                // If we have a graphics device, tell the screen to load content.
                if (_isInitialized)
                    screen.Activate(false);

                _screens.Add(screen);
                TouchPanel.EnabledGestures = screen.EnabledGestures;
            }
        }

        // Removes a screen from the screen manager. You should normally
        // use GameScreen.ExitScreen instead of calling this directly, so
        // the screen can gradually transition off rather than just being
        // instantly removed.
        public void RemoveScreen(Scene screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (_isInitialized)
                screen.Unload();

            _screens.Remove(screen);
            _tempScreensList.Remove(screen);

            // if there is a screen still in the manager, update TouchPanel
            // to respond to gestures that screen is interested in.
            if (_screens.Count > 0)
                TouchPanel.EnabledGestures = _screens[_screens.Count - 1].EnabledGestures;
        }

        // Expose an array holding all the screens. We return a copy rather
        // than the real master list, because screens should only ever be added
        // or removed using the AddScreen and RemoveScreen methods.
        public Scene[] GetScreens()
        {
            return _screens.ToArray();
        }

        // Helper draws a translucent black fullscreen sprite, used for fading
        // screens in and out, and for darkening the background behind popups.
        public void FadeBackBufferToBlack(float alpha)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_blankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            _spriteBatch.End();
        }

        // Informs the screen manager to serialize its state to disk.
        public void Deactivate()
        {
#if WINDOWS_PHONE
            // Open up isolated storage
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Create an XML document to hold the list of screen types currently in the stack
                XDocument doc = new XDocument();
                XElement root = new XElement("ScreenManager");
                doc.Add(root);

                // Make a copy of the master screen list, to avoid confusion if
                // the process of deactivating one screen adds or removes others.
                tempScreensList.Clear();
                foreach (GameScreen screen in screens)
                    tempScreensList.Add(screen);

                // Iterate the screens to store in our XML file and deactivate them
                foreach (GameScreen screen in tempScreensList)
                {
                    // Only add the screen to our XML if it is serializable
                    if (screen.IsSerializable)
                    {
                        // We store the screen's controlling player so we can rehydrate that value
                        string playerValue = screen.ControllingPlayer.HasValue
                            ? screen.ControllingPlayer.Value.ToString()
                            : "";

                        root.Add(new XElement(
                            "GameScreen",
                            new XAttribute("Type", screen.GetType().AssemblyQualifiedName),
                            new XAttribute("ControllingPlayer", playerValue)));
                    }

                    // Deactivate the screen regardless of whether we serialized it
                    screen.Deactivate();
                }

                // Save the document
                using (IsolatedStorageFileStream stream = storage.CreateFile(StateFilename))
                {
                    doc.Save(stream);
                }
            }
#endif
        }

        public bool Activate(bool instancePreserved)
        {
#if !WINDOWS_PHONE
            return false;
#else
            // If the game instance was preserved, the game wasn't dehydrated so our screens still exist.
            // We just need to activate them and we're ready to go.
            if (instancePreserved)
            {
                // Make a copy of the master screen list, to avoid confusion if
                // the process of activating one screen adds or removes others.
                tempScreensList.Clear();

                foreach (GameScreen screen in screens)
                    tempScreensList.Add(screen);

                foreach (GameScreen screen in tempScreensList)
                    screen.Activate(true);
            }

            // Otherwise we need to refer to our saved file and reconstruct the screens that were present
            // when the game was deactivated.
            else
            {
                // Try to get the screen factory from the services, which is required to recreate the screens
                IScreenFactory screenFactory = Game.Services.GetService(typeof(IScreenFactory)) as IScreenFactory;
                if (screenFactory == null)
                {
                    throw new InvalidOperationException(
                        "Game.Services must contain an IScreenFactory in order to activate the ScreenManager.");
                }

                // Open up isolated storage
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // Check for the file; if it doesn't exist we can't restore state
                    if (!storage.FileExists(StateFilename))
                        return false;

                    // Read the state file so we can build up our screens
                    using (IsolatedStorageFileStream stream = storage.OpenFile(StateFilename, FileMode.Open))
                    {
                        XDocument doc = XDocument.Load(stream);

                        // Iterate the document to recreate the screen stack
                        foreach (XElement screenElem in doc.Root.Elements("GameScreen"))
                        {
                            // Use the factory to create the screen
                            Type screenType = Type.GetType(screenElem.Attribute("Type").Value);
                            GameScreen screen = screenFactory.CreateScreen(screenType);

                            // Rehydrate the controlling player for the screen
                            PlayerIndex? controllingPlayer = screenElem.Attribute("ControllingPlayer").Value != ""
                                ? (PlayerIndex)Enum.Parse(typeof(PlayerIndex), screenElem.Attribute("ControllingPlayer").Value, true)
                                : (PlayerIndex?)null;
                            screen.ControllingPlayer = controllingPlayer;

                            // Add the screen to the screens list and activate the screen
                            screen.ScreenManager = this;
                            screens.Add(screen);
                            screen.Activate(false);

                            // update the TouchPanel to respond to gestures this screen is interested in
                            TouchPanel.EnabledGestures = screen.EnabledGestures;
                        }
                    }
                }
            }

            return true;
#endif
        }

        public void AddComponent<T>(T component) where T: SceneSystemComponent
        {
            components.Add(component);
        }

        public void RemoveComponent<T>(T component) where T: SceneSystemComponent
        {
            components.Remove(component);
        }

        public T GetComponent<T>() where T : SceneSystemComponent
        {
            foreach (var control in components)
                if (control is T)
                    return (T)control;

            return null;
        }

        public void SetBackground(string backgroundName)
        {
            BackgroundTexture = Game.Content.Load<Texture2D>("Assets/textures/background/" + backgroundName);
        }
    }
}