using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class ModelFactory
    {
        public enum Options
        {
            None            = 0,
            NoOptimization  = 1,
            EnsureTangents  = 2,
            // Continue with 4, 8, 16, 32, etc.
        }

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
            public Options options;

            public FileEntry(string file, Options options, Model model)
            {
                this.file = file;
                this.options = options;
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

        public Model FromFile(string file, Options options)
        {
            FileEntry needle = new FileEntry(file, options, null);
            FileEntry result = FindInFiles(needle);
            if (result != null)
            {
                return result.model;
            }
            needle.model = CreateModelFromFile(file, options);
            files.Add(needle);
            return needle.model;
        }

        private FileEntry FindInFiles(FileEntry needle)
        {
            FileEntry result = files.Find(delegate(FileEntry item)
            {
                if (needle.file == item.file && needle.options == item.options)
                    return true;
                else
                    return false;
            });
            return result;
        }

        private Model CreateModelFromFile(string file, Options options)
        {
            ExtendedMaterial[] materials;
            IMesh mesh = factory.MeshFromFile(device, file, out materials);
            HandleOptions(ref mesh, options);
            Model model = new Model(mesh, materials);
            return model;
        }

        private void HandleOptions(ref IMesh mesh, Options options)
        {
            if (!DeclarationHasTangents(mesh.Declaration) &&
                (options & Options.EnsureTangents) == Options.EnsureTangents)
            {
                if (!DeclarationHasNormals(mesh.Declaration))
                {
                    AddNormalsAndTangents(ref mesh);
                }
                else
                {
                    AddTangents(ref mesh);
                }
            }
            else if (!DeclarationHasNormals(mesh.Declaration))
            {
                AddNormals(ref mesh);
            }
            if ((options & Options.NoOptimization) != Options.NoOptimization)
            {
                int[] adj = new int[mesh.NumberFaces * 3];
                mesh.GenerateAdjacency(1e-6f, adj);
                mesh.OptimizeInPlace(MeshFlags.OptimizeVertexCache | MeshFlags.OptimizeStripeReorder | MeshFlags.OptimizeAttributeSort, adj);
            }
        }

        public void AddNormalsAndTangents(ref IMesh mesh)
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(0, 24, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(0, 36, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
                VertexElement.VertexDeclarationEnd,
            };
            IMesh tempMesh = mesh.Clone(MeshFlags.Managed, elements, device);
            mesh.Dispose();
            mesh = tempMesh;
            mesh.ComputeNormals();
            mesh.ComputeTangentFrame(TangentOptions.GenerateInPlace | TangentOptions.WeightEqual);
        }

        public void AddTangents(ref IMesh mesh)
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(0, 24, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                new VertexElement(0, 36, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
                VertexElement.VertexDeclarationEnd,
            };
            IMesh tempMesh = mesh.Clone(MeshFlags.Managed, elements, device);
            mesh.Dispose();
            mesh = tempMesh;
            mesh.ComputeTangentFrame(TangentOptions.GenerateInPlace | TangentOptions.WeightEqual);
        }

        public void AddNormals(ref IMesh mesh)
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(0, 24, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                VertexElement.VertexDeclarationEnd,
            };
            IMesh tempMesh = mesh.Clone(MeshFlags.Managed, elements, device);
            mesh.Dispose();
            mesh = tempMesh;
            mesh.ComputeNormals();
        }

        private bool DeclarationHasTangents(VertexElement[] vertexElement)
        {
            for (int i = 0; i < vertexElement.Length; i++)
            {
                if (vertexElement[i].DeclarationUsage == DeclarationUsage.Tangent)
                    return true;
            }
            return false;
        }

        private bool DeclarationHasNormals(VertexElement[] vertexElement)
        {
            for (int i = 0; i < vertexElement.Length; i++)
            {
                if (vertexElement[i].DeclarationUsage == DeclarationUsage.Normal)
                    return true;
            }
            return false;
        }

        //public void AutoExpire()
        //{
        //    boxes.RemoveAll(delegate(BoxEntry item) { if (!item.meshReference.IsAlive) return true; return false; });
        //    files.RemoveAll(delegate(FileEntry item) { if (!item.meshReference.IsAlive) return true; return false; });
        //}
    }
}
