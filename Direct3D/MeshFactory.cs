using System;
using System.Collections.Generic;
using System.Text;

namespace Direct3D
{
    public class MeshFactory
    {
        private class BoxEntry
        {
            public float width;
            public float height;
            public float depth;
            public WeakReference meshReference;

            public BoxEntry(float width, float height, float depth, IMesh mesh)
            {
                this.width = width;
                this.height = height;
                this.depth = depth;
                if (mesh != null)
                    this.meshReference = new WeakReference(mesh);
            }
        }

        private IFactory factory;
        private IDevice device;

        private List<BoxEntry> boxes = new List<BoxEntry>();

        public enum Usage
        {
            Static,
            Dynamic
        }

        public MeshFactory(IDevice device, IFactory factory)
        {
            this.device = device;
            this.factory = factory;
        }

        public int CountBoxes { get { return boxes.Count; } }
        public int Count { get { return CountBoxes; } }

        public IMesh CreateBox(float width, float height, float depth, Usage usage)
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
                return (IMesh)result.meshReference.Target;
            }
            IMesh mesh = factory.CreateBoxMesh(device, width, height, depth);
            needle.meshReference = new WeakReference(mesh);
            boxes.Add(needle);
            return mesh;
        }

        private static bool DeadNode(BoxEntry item)
        {
            if (!item.meshReference.IsAlive)
            {
                return true;
            }
            return false;
        }

        public void AutoExpire()
        {
            GC.Collect();
            boxes.RemoveAll(DeadNode);
        }
    }
}
