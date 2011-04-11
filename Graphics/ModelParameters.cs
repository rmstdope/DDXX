using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class ModelParameters
    {
        private string name;
        private string effect;
        private CustomModel model;

        public string Name
        {
            get { return name; }
        }

        public string Effect
        {
            get { return effect; }
        }

        public CustomModel Model
        {
            get { return model; }
        }

        public bool IsGenerated
        {
            get { return false; }
        }

        public ModelParameters(string name, string effect, CustomModel model)
        {
            this.name = name;
            this.effect = effect;
            this.model = model;
        }
    }
}
