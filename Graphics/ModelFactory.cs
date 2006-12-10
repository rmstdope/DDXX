using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

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
        private ITextureFactory textureFactory;

        private List<BoxEntry> boxes = new List<BoxEntry>();
        private List<FileEntry> files = new List<FileEntry>();

        public ModelFactory(IDevice device, IGraphicsFactory factory, ITextureFactory textureFactory)
        {
            this.device = device;
            this.factory = factory;
            this.textureFactory = textureFactory;
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
            ModelMaterial[] materials = new ModelMaterial[1];
            materials[0] = new ModelMaterial(new Material());
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
            ExtendedMaterial[] extendedMaterials;
            IMesh mesh = factory.MeshFromFile(device, file, out extendedMaterials);
            ModelMaterial[] modelMaterials = new ModelMaterial[extendedMaterials.Length];
            for (int i = 0; i < extendedMaterials.Length; i++)
            {
                if (extendedMaterials[i].TextureFilename == null ||
                    extendedMaterials[i].TextureFilename == "")
                    modelMaterials[i] = new ModelMaterial(extendedMaterials[i].Material3D);
                else
                    modelMaterials[i] = new ModelMaterial(extendedMaterials[i].Material3D, textureFactory.CreateFromFile(extendedMaterials[i].TextureFilename));
                Material material = modelMaterials[i].Material;
                material.AmbientColor = material.DiffuseColor;
                modelMaterials[i].Material = material;
            }
            HandleOptions(ref mesh, options);
            Model model = new Model(mesh, modelMaterials);
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
            Optimize(mesh, options);
        }

        private static void Optimize(IMesh mesh, Options options)
        {
            if ((options & Options.NoOptimization) != Options.NoOptimization)
            {
                int[] adj = new int[mesh.NumberFaces * 3];
                mesh.GenerateAdjacency(1e-6f, adj);
                mesh.OptimizeInPlace(MeshFlags.OptimizeStripeReorder | MeshFlags.OptimizeAttributeSort, adj);
            }
        }

        private void AddNormalsAndTangents(ref IMesh mesh)
        {
            VertexElementArray elements = new VertexElementArray(mesh.Declaration);
            elements.AddNormals();
            elements.AddTangents();
            elements.AddBiNormals();
            IMesh tempMesh = mesh.Clone(MeshFlags.Managed, elements.VertexElements, device);
            mesh.Dispose();
            mesh = tempMesh;
            mesh.ComputeNormals();
            mesh.ComputeTangentFrame(TangentOptions.GenerateInPlace | TangentOptions.WeightEqual);
        }

        private void AddTangents(ref IMesh mesh)
        {
            VertexElementArray elements = new VertexElementArray(mesh.Declaration);
            elements.AddTangents();
            elements.AddBiNormals();
            IMesh tempMesh = mesh.Clone(MeshFlags.Managed, elements.VertexElements, device);
            mesh.Dispose();
            mesh = tempMesh;
            int[] adjacency = new int[mesh.NumberFaces * 3];
            int[] adjacencyOut;
            mesh.GenerateAdjacency(1e-6f, adjacency);
            tempMesh = mesh.Clean(CleanType.BowTies | CleanType.BackFacing, adjacency, out adjacencyOut);
            if (mesh != tempMesh)
            {
                mesh.Dispose();
                mesh = tempMesh;
            }
            mesh.ComputeTangent(0, 0, 0, 0);
            //mesh.ComputeTangentFrame(TangentOptions.GenerateInPlace | TangentOptions.WeightEqual | TangentOptions.DontOrthogonalize);
        }

        private void AddNormals(ref IMesh mesh)
        {
            VertexElement[] elements = AddDeclarationElement(mesh.Declaration, new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0));
            IMesh tempMesh = mesh.Clone(MeshFlags.Managed, elements, device);
            mesh.Dispose();
            mesh = tempMesh;
            mesh.ComputeNormals();
        }

        private VertexElement[] AddDeclarationElement(VertexElement[] elements, VertexElement element)
        {
            VertexElement[] newElements = new VertexElement[elements.Length + 1];
            short offset = 0;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                newElements[i] = elements[i];
                offset = elements[i].Offset;
            }
            switch (elements[elements.Length - 2].DeclarationType)
            {
                case DeclarationType.Float1:
                    offset += 4;
                    break;
                case DeclarationType.Float2:
                    offset += 8;
                    break;
                case DeclarationType.Float3:
                    offset += 12;
                    break;
                case DeclarationType.Float4:
                    offset += 16;
                    break;
                case DeclarationType.Color:
                    offset += 4;
                    break;
                default:
                    throw new DDXXException("DeclarationType not implemented: " + elements[elements.Length - 2].DeclarationType.ToString());
            }
            newElements[elements.Length - 1] = element;
            newElements[elements.Length - 1].Offset = offset;
            newElements[elements.Length] = VertexElement.VertexDeclarationEnd;
            return newElements;
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
    }
}
