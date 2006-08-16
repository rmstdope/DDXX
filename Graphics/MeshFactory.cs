using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class MeshFactory
    {
        private class BoxEntry
        {
            public float width;
            public float height;
            public float depth;
            public WeakReference meshReference;

            public BoxEntry(float width, float height, float depth, MeshContainer mesh)
            {
                this.width = width;
                this.height = height;
                this.depth = depth;
                if (mesh != null)
                    this.meshReference = new WeakReference(mesh);
            }
        }
        private class FileEntry
        {
            public string file;
            public WeakReference meshReference;

            public FileEntry(string file, MeshContainer mesh)
            {
                this.file = file;
                if (mesh != null)
                    this.meshReference = new WeakReference(mesh);
            }
        }

        private IGraphicsFactory factory;
        private IDevice device;

        private List<BoxEntry> boxes = new List<BoxEntry>();
        private List<FileEntry> files = new List<FileEntry>();

        public MeshFactory(IDevice device, IGraphicsFactory factory)
        {
            this.device = device;
            this.factory = factory;
        }

        public int CountBoxes { get { return boxes.Count; } }
        public int CountFiles { get { return files.Count; } }
        public int Count { get { return CountBoxes + CountFiles; } }

        public MeshContainer CreateBox(float width, float height, float depth)
        {
            AutoExpire();

            BoxEntry needle = new BoxEntry(width, height, depth, null);
            BoxEntry result = boxes.Find(delegate (BoxEntry item)
            {
                if (needle.width == item.width &&
                    needle.height == item.height &&
                    needle.depth == item.depth) 
                    return true; 
                else 
                    return false; 
            });
            if (result != null)
            {
                return (MeshContainer)result.meshReference.Target;
            }
            EffectInstance[] instance = new EffectInstance[1];
            instance[0] = new EffectInstance();
            MeshContainer mesh = new MeshContainer(factory.CreateBoxMesh(device, width, height, depth), instance);
            needle.meshReference = new WeakReference(mesh);
            boxes.Add(needle);
            return mesh;
        }

        public MeshContainer FromFile(string file)
        {
            AutoExpire();

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
                return (MeshContainer)result.meshReference.Target;
            }
            EffectInstance[] effects;
            IMesh mesh = factory.MeshFromFile(device, file, out effects);
            MeshContainer container = new MeshContainer(mesh, effects);
            needle.meshReference = new WeakReference(container);
            files.Add(needle);
            return container;
        }

        public void AutoExpire()
        {
            boxes.RemoveAll(delegate(BoxEntry item) { if (!item.meshReference.IsAlive) return true; return false; });
            files.RemoveAll(delegate(FileEntry item) { if (!item.meshReference.IsAlive) return true; return false; });
        }
    }
}
