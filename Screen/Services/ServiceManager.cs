using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Services
{
    public class ServiceManager : Screen.Components.SceneSystemComponent
    {
        List<IService> _services;

        ServiceManagerState state = ServiceManagerState.Idel;

        public ServiceManagerState State => state;

        public ServiceManager(Game game) : base(game)
        {
            _services = new List<IService>();
            state = ServiceManagerState.Idel;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            state = ServiceManagerState.UpdatingServices;
            for (int i = 0; i < _services.Count; i++)
                _services[i].Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            for (int i = 0; i < _services.Count; i++)
                if (_services[i] is IService2)
                    ((IService2)_services[i]).Draw(spriteBatch, gameTime);
        }


        public IService this[string name]=>GetService(name);

        public IService GetService(string name) 
        {
            IService service = null;
            foreach (var serv in _services)
                if (serv.Name == name)
                    service = serv;
            return service;
        }

        public T GetService<T>() where T: IService
        {
            IService service = null;
            foreach (var serin in _services)
                if (serin is T)
                    service = serin;
            return (T)service;
        }
        public void AddService(IService service)
        {
            if (!_services.Contains(service))
            {
                state = ServiceManagerState.AddingService;
                _services.Add(service);
            }
        }

        public void RemoveService(IService service)
        {
            if(_services.Contains(service))
            {
                state = ServiceManagerState.RemovingService;
                _services.Remove(service);
            }
        }
    }
}
