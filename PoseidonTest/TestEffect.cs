using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using System.Diagnostics;
using System.Drawing;

namespace PoseidonTest
{
    public class RealRenderPostEffect : BaseDemoPostEffect
    {
        //private Scene scene;
        private PosseTestEffect demoEffect;
        public RealRenderPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public void SetPosseTestEffect(PosseTestEffect posseEffect)
        {
            this.demoEffect = posseEffect;
        }

        public override void Render()
        {
            TextureID tempTextureID = TextureID.FULLSIZE_TEXTURE_1;
            TextureID tempTextureID2 = TextureID.FULLSIZE_TEXTURE_2;
            if (PostProcessor.OutputTextureID == TextureID.FULLSIZE_TEXTURE_1)
            {
                tempTextureID = TextureID.FULLSIZE_TEXTURE_3;
            }
            else if (PostProcessor.OutputTextureID == TextureID.FULLSIZE_TEXTURE_2)
            {
                tempTextureID2 = TextureID.FULLSIZE_TEXTURE_3;
            }

            //surfaceloader.fromsurface(temptexture.getsurfacelevel(0).dxsurface,
            //    postprocessor.outputtexture.getsurfacelevel(0).dxsurface,
            //    filter.none, 0);

            //demoEffect.SourceTexture = tempTexture;

            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("DownSample4x", PostProcessor.OutputTextureID, tempTextureID);
            PostProcessor.Process("DownSample4x", tempTextureID, tempTextureID2);

            PostProcessor.Process("DownSample4x", tempTextureID2, tempTextureID);

            demoEffect.SourceTexture = PostProcessor.GetTexture(tempTextureID2);

            //((PostProcessor)PostProcessor).DebugWriteAllTextures();
            
            ISurface sourceSurface = PostProcessor.GetTexture(tempTextureID2).GetSurfaceLevel(0);
            SurfaceDescription description = sourceSurface.Description;
            using (ISurface offscreenTarget = demoEffect.GetDevice().CreateOffscreenPlainSurface(
                description.Width, description.Height, description.Format, Pool.SystemMemory))
            {
                demoEffect.GetDevice().GetRenderTargetData(sourceSurface, offscreenTarget);

                int pitch;
                GraphicsStream stream = offscreenTarget.LockRectangle(LockFlags.ReadOnly, out pitch);
                byte[] buffer = new byte[description.Width * description.Height * 4];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                offscreenTarget.UnlockRectangle();
                for (int i = 0; i < demoEffect.HeightMapX; i++)
                {
                    for (int j = 0; j < demoEffect.HeightMapY; j++)
                    {
                        int sourceIndex = j*pitch + i*4;
                        int destIndex = j*demoEffect.HeightMapX + i;
                        demoEffect.HeightMap[destIndex] = buffer[sourceIndex + 2];
                    }
                }
            }

            using (ISurface oldTarget = demoEffect.GetDevice().GetRenderTarget(0))
            {
                using (ISurface newTarget = PostProcessor.OutputTexture.GetSurfaceLevel(0))
                {
                    demoEffect.GetDevice().SetRenderTarget(0, newTarget);
                    demoEffect.GetDevice().Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Blue, 1000, 0);
                    demoEffect.GetDevice().BeginScene();
                    demoEffect.RealRender();
                    demoEffect.GetDevice().EndScene();
                    demoEffect.GetDevice().SetRenderTarget(0, oldTarget);
                }
            }
        }

    }

    public class PosseTestEffect : BaseDemoEffect
    {
        private ModelNode virtualModelNode;
        private ModelNode modelNode;
        private Scene scene;
        private Scene virtualScene;
        private IEffect effect;
        private EffectHandle sourceTextureHandle;
        private EffectHandle heightMapHandle;
        private EffectHandle zMaxHandle;
        private EffectHandle zMinHandle;
        private int heightMapMaxSize;
        private float[] heightMap;
        ModelNode[,] modelNodes;
        int xmax;
        int ymax;
        private float zScale = 0.003f;

        public PosseTestEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
            scene = new Scene();
            virtualScene = new Scene();
        }

        public ITexture SourceTexture
        {
            set
            {
                effect.SetValue(sourceTextureHandle, value);
            }
        }

        public int HeightMapX
        {
            get { return xmax; }
        }
        public int HeightMapY
        {
            get { return ymax; }
        }
        public float[] HeightMap
        {
            get { return heightMap; }
        }

        public override void Initialize()
        {
            base.Initialize();

            InitializeVirtualScene();

            sourceTextureHandle = effect.GetParameter(null, "PosseSourceTexture");
            heightMapHandle = effect.GetParameter(null, "HeightMap");
            EffectHandle sizeHandle = effect.GetParameter(null, "HeightMapMaxSize");
            heightMapMaxSize = effect.GetValueInteger(sizeHandle);
            zMaxHandle = effect.GetParameter(null, "ZMAX");
            zMinHandle = effect.GetParameter(null, "ZMIN");
            InitializeScene();
        }

        public float ZMax
        {
            get { return effect.GetValueFloat(zMaxHandle); }
            set { effect.SetValue(zMaxHandle, value); }
        }

        public float ZMin
        {
            get { return effect.GetValueFloat(zMinHandle); }
            set { effect.SetValue(zMinHandle, value); }
        }

        private void InitializeVirtualScene()
        {
            ModelMaterial material = new ModelMaterial(new Material());
            //material.DiffuseTexture = TextureFactory.CreateFromFile("wings.bmp");
            // targetTexture = TextureFactory.CreateFullsizeRenderTarget();
            material.DiffuseColor = ColorValue.FromColor(Color.Blue);
            material.Ambient = Color.SteelBlue;
            IModel model = ModelFactory.CreateBox(50, 50, 50);
            model.Materials = new ModelMaterial[] { material };

            effect = EffectFactory.CreateFromFile("../../Effects/PosseTest.fxo");
            EffectHandler effectHandler = new EffectHandler(effect, "NoTex", model);
            virtualModelNode = new ModelNode("Mesh", model, effectHandler);
            virtualScene.AddNode(virtualModelNode);
            virtualScene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //Light dxLight = new Light();
            //dxLight.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //dxLight.Type = LightType.Point;
            //light = new LightNode("Point Light", dxLight);
            //scene.AddNode(light);

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-150.0f);
            virtualScene.AddNode(camera);
            virtualScene.ActiveCamera = camera;
        }

        private void InitializeScene()
        {
            ModelMaterial material = new ModelMaterial(new Material());
            material.DiffuseTexture = TextureFactory.CreateFromFile("wings.bmp");
            material.DiffuseColor = ColorValue.FromColor(Color.Blue);
            material.Ambient = Color.White;
            SourceTexture = material.DiffuseTexture;
            ModelMaterial[] modelMaterials = new ModelMaterial[] { material };
            IModel model = TexturedBox(ModelFactory.CreateBox(100, 100, 1), modelMaterials);
            IEffect effect = EffectFactory.CreateFromFile("../../Effects/PosseTest.fxo");
            EffectHandler effectHandler = new EffectHandler(effect, "Tex", model);
            modelNode = new ModelNode("Mesh", model, effectHandler);
            modelNode.WorldState.Tilt(4.0f);
            //scene.AddNode(modelNode);
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-150.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            xmax = Device.DisplayMode.Width / 16; // downsample4 x 2
            ymax = Device.DisplayMode.Height / 16; // downsample4 x 2
            xmax = 40;
            ymax = 40;
            modelNodes = new ModelNode[xmax, ymax];
            heightMap = new float[xmax * ymax];

            DummyNode pinBoardNode = new DummyNode("PinBoard");
            int boxWidth = 2;
            int boxSpace = 4;
            float zSize = 1.0f;
            IModel pinModel = TexturedBox(ModelFactory.CreateBox(boxWidth, boxWidth, zSize), modelMaterials, zSize/2.0f);
            for (int x = 0; x < xmax; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    ModelNode node = new ModelNode("PinModelNode", pinModel, effectHandler);
                    float posx = x * boxSpace - xmax * boxSpace / 2;
                    float posy = y * boxSpace - ymax * boxSpace / 2;
                    heightMap[y * xmax + x] = 0;
                    node.WorldState.Position = new Vector3(posx, posy, 0);
                    modelNodes[x, y] = node;
                    pinBoardNode.AddChild(node);
                }
            }
            pinBoardNode.WorldState.MoveForward(50.0f);
            pinBoardNode.WorldState.Tilt(4.0f);
            pinBoardNode.WorldState.Roll(4.0f);
            scene.AddNode(pinBoardNode);
        }

        public float ZScale
        {
            get { return zScale; }
            set { zScale = value; }
        }

        public void UpdatePins()
        {
            for (int x = 0; x < xmax; x++)
            {
                for (int y = 0; y < ymax; y++)
                {
                    Vector3 scaling = modelNodes[x, y].WorldState.Scaling;
                    scaling.Z = ZScale * heightMap[y * xmax + x];
                    modelNodes[x, y].WorldState.Scaling = scaling;
                }
            }
        }

        private IMesh CreateBox()
        {
            IMesh mesh;
            short[] arrayIndices = new short[12];
            CustomVertex.PositionNormal[] arrayVertices = new CustomVertex.PositionNormal[8];
            mesh = D3DDriver.GraphicsFactory.CreateMesh(4, 8, MeshFlags.Managed, CustomVertex.PositionNormal.Format, Device);

            arrayVertices[0] = new CustomVertex.PositionNormal(-10, -10, -10, 0, 0, -1);
            arrayVertices[1] = new CustomVertex.PositionNormal(10, -10, -10, 0, 0, -1);
            arrayVertices[2] = new CustomVertex.PositionNormal(10, 10, -10, 0, 0, -1);
            arrayVertices[3] = new CustomVertex.PositionNormal(-10, 10, -10, 0, 0, -1);
            arrayVertices[4] = new CustomVertex.PositionNormal(10, -10, 10, 0, 0, 1);
            arrayVertices[5] = new CustomVertex.PositionNormal(-10, -10, 10, 0, 0, 1);
            arrayVertices[6] = new CustomVertex.PositionNormal(-10, 10, 10, 0, 0, 1);
            arrayVertices[7] = new CustomVertex.PositionNormal(10, 10, 10, 0, 0, 1);
            int i = 0;
            arrayIndices[i++] = 0;
            arrayIndices[i++] = 3;
            arrayIndices[i++] = 1;
            arrayIndices[i++] = 1;
            arrayIndices[i++] = 3;
            arrayIndices[i++] = 2;
            arrayIndices[i++] = 4;
            arrayIndices[i++] = 7;
            arrayIndices[i++] = 5;
            arrayIndices[i++] = 5;
            arrayIndices[i++] = 7;
            arrayIndices[i++] = 6;
            AttributeRange attributeRange = new AttributeRange();
            attributeRange.AttributeId = 0;
            attributeRange.FaceCount = 4;
            attributeRange.FaceStart = 0;
            attributeRange.VertexStart = 0;
            attributeRange.VertexCount = 8;

            mesh.VertexBuffer.SetData(arrayVertices, 0, LockFlags.None);
            mesh.IndexBuffer.SetData(arrayIndices, 0, LockFlags.None);
            mesh.SetAttributeTable(new AttributeRange[] { attributeRange });

            return mesh;
        }

        private Model TexturedBox(IModel model, ModelMaterial[] materials)
        {
            return TexturedBox(model, materials, 0.0f);
        }

        private Model TexturedBox(IModel model, ModelMaterial[] materials, float zOffset)
        {
            IMesh mesh = model.Mesh;
            Debug.Assert(mesh.NumberFaces == 12 && mesh.NumberVertices == 24);
            IMesh texturedMesh = mesh.Clone(mesh.Options.Value,
                VertexFormats.Position | VertexFormats.Normal |
                VertexFormats.Texture0 | VertexFormats.Texture1,
                new DeviceAdapter(mesh.Device));
            SetBoxCoordinates(texturedMesh, zOffset);
            return new Model(texturedMesh, materials);
        }

        private void SetBoxCoordinates(IMesh texturedMesh, float zOffset)
        {
            using (IVertexBuffer vb = texturedMesh.VertexBuffer)
            {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, texturedMesh.NumberVertices);
                try
                {
                    for (int i = 0; i < verts.Length; i += 4)
                    {
                        verts[i + 0].Tu = 0.0f;
                        verts[i + 0].Tv = 0.0f;
                        verts[i + 1].Tu = 1.0f;
                        verts[i + 1].Tv = 0.0f;
                        verts[i + 2].Tu = 1.0f;
                        verts[i + 2].Tv = 1.0f;
                        verts[i + 3].Tu = 0.0f;
                        verts[i + 3].Tv = 1.0f;

                        verts[i + 0].Z += zOffset;
                        verts[i + 1].Z += zOffset;
                        verts[i + 2].Z += zOffset;
                        verts[i + 3].Z += zOffset;
                    }
                }
                finally
                {
                    vb.Unlock();
                }
            }
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            scene.Step();
            virtualScene.Step();
            //virtualModelNode.WorldState.Tilt(Time.DeltaTime * 0.3f);
            //virtualModelNode.WorldState.Roll(Time.DeltaTime * 0.4f);
            Vector3 pos = virtualModelNode.WorldState.Position;
            pos.X = 50.0f * (float)Math.Sin(Time.StepTime);
            pos.Y = 50.0f * (float)Math.Cos(Time.StepTime);
            //virtualModelNode.WorldState.Position = pos;
            virtualModelNode.WorldState.Turn(Time.DeltaTime * 0.3f);
            //for (int x = 0; x < xmax; x++)
            //{
            //    for (int y = 0; y < ymax; y++)
            //    {
            //        heightMap[y * xmax + x] = (50.0f +
            //            50.0f * (float)Math.Sin(Time.StepTime + x * 0.05f + y * 0.05f));
            //    }
            //}
            UpdatePins();
            //virtualModelNode.WorldState.Roll(Time.DeltaTime * 0.5f);
            //virtualModelNode.WorldState.Turn(Time.DeltaTime * 0.3f);
            //mesh.WorldState.Turn(Time.DeltaTime * 0.5f);
            //light.WorldState.Position = new Vector3(500.0f * (float)Math.Sin(Time.StepTime),
            //                                        0.0f,
            //                                        500.0f * (float)Math.Cos(Time.StepTime));
        }

        public override void Render()
        {
            virtualScene.Render();
        }

        public void RealRender()
        {
            scene.Render();
        }

        public IDevice GetDevice()
        {
            return Device;
        }
    }
}
