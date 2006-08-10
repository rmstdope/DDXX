using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public class EffectFactory
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

        private IFactory factory;
        private IDevice device;
        private List<FileEntry> files = new List<FileEntry>();
        private EffectPool pool;

        public EffectFactory(IDevice device, IFactory factory)
        {
            this.device = device;
            this.factory = factory;
            pool = new EffectPool();
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
                return result.effect;
            }
            ShaderFlags flags = ShaderFlags.None;
#if __DEBUG__
            flags |= (ShaderFlags.Debug | ShaderFlags.SkipOptimization);
#endif
            IEffect effect = factory.CreateEffectFromFile(device, file, null, "", flags, pool);
            needle.effect = effect;
            files.Add(needle);
            return effect;

        }
    }
}
