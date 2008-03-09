using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Graphics
{
    public class ModelParameters
    {
        private string name;
        private string effect;
        private IModel model;

        public string Name
        {
            get { return name; }
        }

        public string Effect
        {
            get { return effect; }
        }

        public IModel Model
        {
            get { return model; }
        }

        public ModelParameters(string name, string effect, IModel model)
        {
            this.name = name;
            this.effect = effect;
            this.model = model;
        }
    }
}
