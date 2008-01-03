using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectFactory : IEffectFactory
    {
        private class FileEntry
        {
            public string file;
            public IEffect effect;

            public FileEntry(string file, IEffect effect)
            {
                this.file = file;
                this.effect = effect;
            }
        }

        private IGraphicsFactory factory;
        private IGraphicsDevice device;
        private List<FileEntry> files = new List<FileEntry>();
        //private EffectPool pool;

        public EffectFactory(IGraphicsDevice device, IGraphicsFactory factory)
        {
            this.device = device;
            this.factory = factory;
            //pool = new EffectPool();
        }

        public IEffect CreateFromFile(string file)
        {
            FileEntry needle = new FileEntry(file, null);
            FileEntry result = files.Find(delegate(FileEntry item)
            {
                if (needle.file == item.file)
                    return true;
                else
                    return false;
            });
            if (result != null)
            {
                return result.effect.Clone(result.effect.GraphicsDevice);
                //return result.effect;
            }
            IEffect effect = factory.EffectFromFile(file);
            needle.effect = effect;
            files.Add(needle);
            return effect;

        }
    }
}
