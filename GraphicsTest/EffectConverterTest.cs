using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class EffectConverterTest
    {
        private Mockery mockery;
        private IEffectConverter converter;
        private IEffectParameter fromFloatParameter;
        private IEffectParameter toFloatParameter;
        private IEffectParameter fromTextureParameter;
        private IEffectParameter toTextureParameter;
        private ITexture2D texture2D;
        private ITextureCube textureCube;
        private IEffect oldEffect;
        private IEffect newEffect;
        private ICollectionAdapter<IEffectParameter> fromCollection;
        private ICollectionAdapter<IEffectParameter> toCollection;
        private IEnumerator<IEffectParameter> fromEnumerator;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            oldEffect = mockery.NewMock<IEffect>();
            newEffect = mockery.NewMock<IEffect>();
            fromFloatParameter = mockery.NewMock<IEffectParameter>();
            toFloatParameter = mockery.NewMock<IEffectParameter>();
            fromTextureParameter = mockery.NewMock<IEffectParameter>();
            toTextureParameter = mockery.NewMock<IEffectParameter>();
            texture2D = mockery.NewMock<ITexture2D>();
            textureCube = mockery.NewMock<ITextureCube>();

            converter = new EffectConverter();

            fromCollection = mockery.NewMock<ICollectionAdapter<IEffectParameter>>();
            fromEnumerator = mockery.NewMock<IEnumerator<IEffectParameter>>();
            Stub.On(oldEffect).GetProperty("Parameters").Will(Return.Value(fromCollection));
            Stub.On(fromCollection).Method("GetEnumerator").Will(Return.Value(fromEnumerator));
            Stub.On(fromEnumerator).Method("Dispose");

            toCollection = mockery.NewMock<ICollectionAdapter<IEffectParameter>>();
            Stub.On(newEffect).GetProperty("Parameters").Will(Return.Value(toCollection));
        }

        [Test]
        public void SingleFloatNoDestination()
        {
            // Setup
            ExpectForeachOnParameters(new IEffectParameter[] { fromFloatParameter });
            Stub.On(fromFloatParameter).GetProperty("Name").Will(Return.Value("Name"));
            Stub.On(toCollection).Method("get_Item").With("Name").Will(Return.Value(null));

            //Exercise SUT
            converter.Convert(oldEffect, newEffect);
        }

        [Test]
        public void SingleFloat()
        {
            // Setup
            ExpectForeachOnParameters(new IEffectParameter[] { fromFloatParameter });
            Stub.On(fromFloatParameter).GetProperty("Name").Will(Return.Value("Name"));
            Stub.On(toCollection).Method("get_Item").With("Name").Will(Return.Value(toFloatParameter));
            Stub.On(fromFloatParameter).GetProperty("ParameterType").Will(Return.Value(EffectParameterType.Single));
            Stub.On(fromFloatParameter).GetProperty("ColumnCount").Will(Return.Value(1));
            Stub.On(fromFloatParameter).GetProperty("RowCount").Will(Return.Value(1));
            Stub.On(fromFloatParameter).Method("GetValueSingleArray").With(1).Will(Return.Value(new float[] { 1.2f }));
            Expect.Once.On(toFloatParameter).Method("SetValue").With(new float[] { 1.2f });

            //Exercise SUT
            converter.Convert(oldEffect, newEffect);
        }

        [Test]
        public void TwoFloats()
        {
            // Setup
            ExpectForeachOnParameters(new IEffectParameter[] { fromFloatParameter });
            Stub.On(fromFloatParameter).GetProperty("Name").Will(Return.Value("Name"));
            Stub.On(toCollection).Method("get_Item").With("Name").Will(Return.Value(toFloatParameter));
            Stub.On(fromFloatParameter).GetProperty("ParameterType").Will(Return.Value(EffectParameterType.Single));
            Stub.On(fromFloatParameter).GetProperty("ColumnCount").Will(Return.Value(2));
            Stub.On(fromFloatParameter).GetProperty("RowCount").Will(Return.Value(1));
            Stub.On(fromFloatParameter).Method("GetValueSingleArray").With(2).Will(Return.Value(new float[] { 1, 2 }));
            Expect.Once.On(toFloatParameter).Method("SetValue").With(new float[] { 1, 2 });

            //Exercise SUT
            converter.Convert(oldEffect, newEffect);
        }

        [Test]
        public void Texture2D()
        {
            // Setup
            ExpectForeachOnParameters(new IEffectParameter[] { fromTextureParameter });
            Stub.On(fromTextureParameter).GetProperty("Name").Will(Return.Value("Name"));
            Stub.On(toCollection).Method("get_Item").With("Name").Will(Return.Value(toTextureParameter));
            Stub.On(fromTextureParameter).GetProperty("ParameterType").Will(Return.Value(EffectParameterType.Texture2D));
            Stub.On(fromTextureParameter).Method("GetValueTexture2D").Will(Return.Value(texture2D));
            Expect.Once.On(toTextureParameter).Method("SetValue").With(texture2D);

            //Exercise SUT
            converter.Convert(oldEffect, newEffect);
        }

        [Test]
        public void TextureCube()
        {
            // Setup
            ExpectForeachOnParameters(new IEffectParameter[] { fromTextureParameter });
            Stub.On(fromTextureParameter).GetProperty("Name").Will(Return.Value("Name"));
            Stub.On(toCollection).Method("get_Item").With("Name").Will(Return.Value(toTextureParameter));
            Stub.On(fromTextureParameter).GetProperty("ParameterType").Will(Return.Value(EffectParameterType.TextureCube));
            Stub.On(fromTextureParameter).Method("GetValueTextureCube").Will(Return.Value(textureCube));
            Expect.Once.On(toTextureParameter).Method("SetValue").With(textureCube);

            //Exercise SUT
            converter.Convert(oldEffect, newEffect);
        }

        [Test]
        public void Matrix()
        {
            // Setup
            ExpectForeachOnParameters(new IEffectParameter[] { fromFloatParameter });
            Stub.On(fromFloatParameter).GetProperty("Name").Will(Return.Value("Name"));
            Stub.On(toCollection).Method("get_Item").With("Name").Will(Return.Value(toFloatParameter));
            Stub.On(fromFloatParameter).GetProperty("ParameterType").Will(Return.Value(EffectParameterType.Single));
            Stub.On(fromFloatParameter).GetProperty("ColumnCount").Will(Return.Value(4));
            Stub.On(fromFloatParameter).GetProperty("RowCount").Will(Return.Value(4));
            Stub.On(fromFloatParameter).Method("GetValueSingleArray").With(16).Will(Return.Value(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 }));
            Expect.Once.On(toFloatParameter).Method("SetValue").With(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 });

            //Exercise SUT
            converter.Convert(oldEffect, newEffect);
        }


        [Test]
        public void FloatAndTexture()
        {
            // Setup
            ExpectForeachOnParameters(new IEffectParameter[] { fromFloatParameter, fromTextureParameter });
            Stub.On(fromFloatParameter).GetProperty("Name").Will(Return.Value("Name1"));
            Stub.On(fromTextureParameter).GetProperty("Name").Will(Return.Value("Name2"));
            Stub.On(toCollection).Method("get_Item").With("Name1").Will(Return.Value(toFloatParameter));
            Stub.On(toCollection).Method("get_Item").With("Name2").Will(Return.Value(toTextureParameter));
            Stub.On(fromFloatParameter).GetProperty("ParameterType").Will(Return.Value(EffectParameterType.Single));
            Stub.On(fromTextureParameter).GetProperty("ParameterType").Will(Return.Value(EffectParameterType.TextureCube));
            Stub.On(fromTextureParameter).Method("GetValueTextureCube").Will(Return.Value(textureCube));
            Stub.On(fromFloatParameter).GetProperty("ColumnCount").Will(Return.Value(1));
            Stub.On(fromFloatParameter).GetProperty("RowCount").Will(Return.Value(1));
            Stub.On(fromFloatParameter).Method("GetValueSingleArray").With(1).Will(Return.Value(new float[] { 5.6f }));
            Expect.Once.On(toTextureParameter).Method("SetValue").With(textureCube);
            Expect.Once.On(toFloatParameter).Method("SetValue").With(new float[] { 5.6f });

            //Exercise SUT
            converter.Convert(oldEffect, newEffect);
        }

        private void ExpectForeachOnParameters(IEffectParameter[] effectParameters)
        {
            using (mockery.Ordered)
            {
                foreach (IEffectParameter parameter in effectParameters)
                {
                    Expect.Once.On(fromEnumerator).Method("MoveNext").Will(Return.Value(true));
                    Expect.Once.On(fromEnumerator).GetProperty("Current").Will(Return.Value(parameter));
                }
                Expect.Once.On(fromEnumerator).Method("MoveNext").Will(Return.Value(false));
            }
        }
    }
}
