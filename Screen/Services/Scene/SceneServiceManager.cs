using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Services.Scene
{
    public sealed class SceneServiceManager
    {
        List<ISceneService> services;
        ISceneServicePrevidor provider;

        public ISceneServicePrevidor Provider => provider;

        /// <summary>
        /// Initialize a new instance of <see cref="SceneServiceManager"/>
        /// </summary>
        /// <param name="provider">The provider of this manager.</param>
        /// <exception cref="NullReferenceException"></exception>
        public SceneServiceManager(ISceneServicePrevidor provider)
        {
            if (provider == null)
                throw new NullReferenceException("provider");

            this.provider = provider;
            this.services = new List<ISceneService>();
        }

        public void AddService<T>(T serviceType) where T: ISceneService
        {
            var service = serviceType as ISceneService;
            service.Initialize();

            services.Add(serviceType);
        }
        public void RemoveService<T>(T serviceType) where T: ISceneService
        {
            services.Remove(serviceType);
        }
        public ISceneService GetService<T>() where T: ISceneService
        {
            ISceneService service = null;
            foreach (var serv in services)
                if (serv is T)
                    service = serv;

            return (T)service;
        }
        public int IndexOf<T>(T serviceType) where T: ISceneService
        {
            return services.IndexOf(serviceType);
        }

        internal void UpdateServices(GameTime gameTime)
        {
            foreach (var item in services)
                item.Update(gameTime);
         }
    }
}
