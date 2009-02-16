using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NMock2;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class D3DMockTest
    {
        protected Mockery mockery;
        protected IGraphicsFactory graphicsFactory;
        protected IGraphicsDevice device;
        protected IRenderTarget2D renderTarget;
        protected ITexture2D texture2D;
        protected ITextureCube textureCube;
        protected IDeviceManager manager;
        protected PresentationParameters presentParameters;
        protected IEffectFactory effectFactory;
        protected ITextureFactory textureFactory;
        protected IEffect effect;
        protected ISpriteBatch spriteBatch;
        protected ISpriteFont spriteFont;
        protected IRenderState renderState;
        protected IVertexBuffer vertexBuffer;
        protected IVertexDeclaration vertexDeclaration;
        protected IVertexStream vertexStream;
        protected IVertexStreamCollection vertexStreamCollection;
        protected IDepthStencilBuffer depthStencilBuffer;
        protected bool verifyExpectations;

        public virtual void SetUp()
        {
            verifyExpectations = true;

            presentParameters = new PresentationParameters();
            presentParameters.BackBufferFormat = SurfaceFormat.Alpha8;
            presentParameters.BackBufferHeight = 200;
            presentParameters.BackBufferWidth = 400;

            mockery = new Mockery();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            effectFactory = mockery.NewMock<IEffectFactory>();
            device = mockery.NewMock<IGraphicsDevice>();
            manager = mockery.NewMock<IDeviceManager>();
            renderTarget = mockery.NewMock<IRenderTarget2D>();
            effect = mockery.NewMock<IEffect>();
            spriteBatch = mockery.NewMock<ISpriteBatch>();
            spriteFont = mockery.NewMock<ISpriteFont>();
            renderState = mockery.NewMock<IRenderState>();
            texture2D = mockery.NewMock<ITexture2D>();
            textureCube = mockery.NewMock<ITextureCube>();
            vertexBuffer = mockery.NewMock<IVertexBuffer>();
            vertexDeclaration = mockery.NewMock<IVertexDeclaration>();
            vertexStreamCollection = mockery.NewMock<IVertexStreamCollection>();
            vertexStream = mockery.NewMock<IVertexStream>();
            depthStencilBuffer = mockery.NewMock<IDepthStencilBuffer>();

            Stub.On(device).GetProperty("PresentationParameters").Will(Return.Value(presentParameters));
            Stub.On(device).GetProperty("RenderState").Will(Return.Value(renderState));
            Stub.On(graphicsFactory).GetProperty("GraphicsDeviceManager").Will(Return.Value(manager));
            Stub.On(graphicsFactory).GetProperty("GraphicsDevice").Will(Return.Value(device));
            Stub.On(graphicsFactory).GetProperty("TextureFactory").Will(Return.Value(textureFactory));
            Stub.On(graphicsFactory).GetProperty("EffectFactory").Will(Return.Value(effectFactory));
            Stub.On(manager).GetProperty("GraphicsDevice").Will(Return.Value(device));
            Stub.On(device).GetProperty("Vertices").Will(Return.Value(vertexStreamCollection));
            Stub.On(vertexStreamCollection).Method("get_Item").With(0).Will(Return.Value(vertexStream));
            Stub.On(effect).GetProperty("GraphicsDevice").Will(Return.Value(device));
        }

        public virtual void TearDown()
        {
            if (verifyExpectations)
                mockery.VerifyAllExpectationsHaveBeenMet();
        }

        protected void StubModelMesh(IModel model, IModelMesh[] modelMeshes)
        {
            List<IModelMesh> modelMeshList = new List<IModelMesh>();
            foreach (IModelMesh modelMesh in modelMeshes)
                modelMeshList.Add(modelMesh);
            ReadOnlyCollection<IModelMesh> modelMeshCollection =
                new ReadOnlyCollection<IModelMesh>(modelMeshList);
            Stub.On(model).GetProperty("Meshes").Will(Return.Value(modelMeshCollection));
        }

        protected void StubModelMeshPart(IModelMesh modelMesh, IModelMeshPart[] modelMeshParts)
        {
            List<IModelMeshPart> modelMeshPartList = new List<IModelMeshPart>();
            foreach (IModelMeshPart part in modelMeshParts)
                modelMeshPartList.Add(part);
            ReadOnlyCollection<IModelMeshPart> modelMeshPartCollection =
                new ReadOnlyCollection<IModelMeshPart>(modelMeshPartList);
            Stub.On(modelMesh).GetProperty("MeshParts").Will(Return.Value(modelMeshPartCollection));
        }

        protected void ExpectForeachPass(IEffectTechnique technique, IEffectPass[] passes)
        {
            ICollectionAdapter<IEffectPass> collection = mockery.NewMock<ICollectionAdapter<IEffectPass>>();
            IEnumerator<IEffectPass> enumerator = mockery.NewMock<IEnumerator<IEffectPass>>();
            Expect.Once.On(technique).GetProperty("Passes").
                Will(Return.Value(collection));
            Expect.Once.On(collection).Method("GetEnumerator").
                Will(Return.Value(enumerator));
            foreach (IEffectPass pass in passes)
            {
                Expect.Once.On(enumerator).Method("MoveNext").Will(Return.Value(true));
                Expect.Once.On(enumerator).GetProperty("Current").Will(Return.Value(pass));
            }
            Expect.Once.On(enumerator).Method("Dispose");
            Expect.Once.On(enumerator).Method("MoveNext").Will(Return.Value(false));
        }

    }
}
