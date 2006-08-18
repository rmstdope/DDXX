using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class ModelFactory
    {
        private class BoxEntry
        {
            public float width;
            public float height;
            public float depth;
            public Model model;

            public BoxEntry(float width, float height, float depth, Model model)
            {
                this.width = width;
                this.height = height;
                this.depth = depth;
                if (model != null)
                    this.model = model;
            }
        }
        private class FileEntry
        {
            public string file;
            public Model model;

            public FileEntry(string file, Model model)
            {
                this.file = file;
                if (model != null)
                    this.model = model;
            }
        }

        private IGraphicsFactory factory;
        private IDevice device;

        private List<BoxEntry> boxes = new List<BoxEntry>();
        private List<FileEntry> files = new List<FileEntry>();

        public ModelFactory(IDevice device, IGraphicsFactory factory)
        {
            this.device = device;
            this.factory = factory;
        }

        public int CountBoxes { get { return boxes.Count; } }
        public int CountFiles { get { return files.Count; } }
        public int Count { get { return CountBoxes + CountFiles; } }

        public Model CreateBox(float width, float height, float depth)
        {
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
                return result.model;
            }
            ExtendedMaterial[] materials = new ExtendedMaterial[1];
            materials[0] = new ExtendedMaterial();
            Model model = new Model(factory.CreateBoxMesh(device, width, height, depth), materials);
            needle.model = model;
            boxes.Add(needle);
            return model;
        }

        public Model FromFile(string file)
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
                return result.model;
            }
            ExtendedMaterial[] materials;
            IMesh mesh = factory.MeshFromFile(device, file, out materials);
            Model model = new Model(mesh, materials);
            needle.model = model;
            files.Add(needle);
            return model;
        }

        //public void AutoExpire()
        //{
        //    boxes.RemoveAll(delegate(BoxEntry item) { if (!item.meshReference.IsAlive) return true; return false; });
        //    files.RemoveAll(delegate(FileEntry item) { if (!item.meshReference.IsAlive) return true; return false; });
        //}
    }
}
