﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Services
{
    public interface IService
    {
        string Name { get; }

        void Update(GameTime gameTime);
    }
}
