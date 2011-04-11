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
            public Effect effect;

            public FileEntry(string file, Effect effect)
            {
                this.file = file;
                this.effect = effect;
            }
        }

        private IGraphicsFactory factory;
        private List<FileEntry> files = new List<FileEntry>();

        public EffectFactory(IGraphicsFactory factory)
        {
            this.factory = factory;
        }

        public BasicEffect CreateBasicEffect()
        {
            return new BasicEffect(factory.GraphicsDevice);
        }

        public Effect CreateFromFile(string file)
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
                return result.effect.Clone();
            }
            Effect effect = factory.EffectFromFile(file);
            needle.effect = effect;
            files.Add(needle);
            return effect;

        }
    }
}
