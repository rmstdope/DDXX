using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using NUnit.Framework;
using Microsoft.CSharp;
using Microsoft.Win32;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoEffectTypesTest
    {
        string source = @"
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework;
public class FooEffect : Registerable, IDemoEffect 
{
  protected int drawOrder; 
  public FooEffect(string s1, float f1, float f2) : base(s1, f1, f2) { }
  public int DrawOrder { get { return drawOrder;} set { drawOrder = value; } }
  public void Step() {} public void Render() {} public void Initialize(IGraphicsFactory graphicsFactory, IEffectFactory effectFactory, ITextureFactory textureFactory, IDemoMixer mixer, IPostProcessor postProcessor) {} 
}
public class BarEffect : FooEffect {
  private int intParam;
  private float floatParam;
  private Vector3 vector3Param;
  public BarEffect(string name, float start, float end) : base(name, start, end) { 
  }
  public int IntParam { 
   get { return intParam; }
   set { intParam = value+1; }
  }
  public float FloatParam { 
   get { return floatParam; }
   set { floatParam = value*2+0.1F; }
  }
  public Vector3 Vector3Param {
   get { return vector3Param; }
   set { vector3Param = value; }
  }
}
public class Dummy {}
public class TestGenerator : Generator
{
  public TestGenerator() : base(0) {}
  public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
  {
    return Vector4.Zero;
  }
}
";
        Assembly assembly;
        CompilerResults results;
        DemoEffectTypes effectTypes;

        [SetUp]
        public void SetUp()
        {
            assembly = SetupAssembly(source);
        }

        [TearDown]
        public void TearDown()
        {
            results.TempFiles.Delete();
        }

        [Test]
        public void TestFindEffectsInAssembly1()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly, null });
            Assert.AreEqual(2, effectTypes.IRegisterables.Count);
            Type t;
            Assert.IsTrue(effectTypes.IRegisterables.TryGetValue("FooEffect", out t));
            Assert.AreEqual(assembly.GetType("FooEffect"), t);
            Assert.IsTrue(effectTypes.IRegisterables.TryGetValue("BarEffect", out t));
            Assert.AreEqual(assembly.GetType("BarEffect"), t);
        }

        [Test]
        public void TestFindEffectsInAssembly2()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            Assert.AreEqual(2, effectTypes.IRegisterables.Count);
            Type t;
            Assert.IsTrue(effectTypes.IRegisterables.TryGetValue("FooEffect", out t));
            Assert.AreEqual(assembly.GetType("FooEffect"), t);
            Assert.IsTrue(effectTypes.IRegisterables.TryGetValue("BarEffect", out t));
            Assert.AreEqual(assembly.GetType("BarEffect"), t);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestCreateInstanceFail()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("fooeffect", "fooname", 0, 1);
            Assert.IsNull(ei);
        }

        [Test]
        public void TestCreateInstanceOK1()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("FooEffect", "FooName", 3, 7);
            Assert.IsNotNull(ei);
            Assert.IsInstanceOfType(assembly.GetType("FooEffect"), ei);
            Assert.AreEqual("FooName", ei.Name);
            Assert.AreEqual(3, ei.StartTime);
            Assert.AreEqual(7, ei.EndTime);
        }

        [Test]
        public void TestCreateInstanceOK2()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", "BarName", 3, 7);
            Assert.IsNotNull(ei);
            Assert.IsInstanceOfType(assembly.GetType("BarEffect"), ei);
            Assert.AreEqual("BarName", ei.Name);
            Assert.AreEqual(3, ei.StartTime);
            Assert.AreEqual(7, ei.EndTime);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetNonExistingProperty()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", "BarName", 0, 1);
            Assert.IsNotNull(ei);
            effectTypes.SetProperty(ei, "XXX", 5);
        }

        [Test]
        public void TestSetIntParameter()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", "BarName", 0, 1);
            Assert.IsNotNull(ei);
            effectTypes.SetProperty(ei, "IntParam", 5);
            int v = (int)effectTypes.GetProperty(ei, "IntParam");
            Assert.AreEqual(6, v);
        }

        [Test]
        public void TestSetFloatParameter()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", "BarName", 0, 1);
            Assert.IsNotNull(ei);
            effectTypes.SetProperty(ei, "FloatParam", 5.5F);
            float v = (float)effectTypes.GetProperty(ei, "FloatParam");
            Assert.AreEqual(11.1F, v);
        }

        private Assembly SetupAssembly(string source)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            string[] sysdir = Environment.SystemDirectory.Split('\\');
            string assemblyDir = string.Join("\\", sysdir, 0, sysdir.Length - 1) + "\\assembly\\";
            AssemblyName[] referenced = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            string d3dAssembly = Assembly.GetAssembly(typeof(Microsoft.Xna.Framework.Vector3)).CodeBase;
            d3dAssembly = d3dAssembly.Remove(0, 8);
            CompilerParameters cp = new CompilerParameters(new string[] { "Dope.DDXX.DemoFramework.dll", "Dope.DDXX.Graphics.dll", "Dope.DDXX.TextureBuilder.dll", "Dope.DDXX.Utility.dll", d3dAssembly });
            results = provider.CompileAssemblyFromSource(cp, source);
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError e in results.Errors)
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                Assert.Fail("Internal error in test code");
            }
            Assert.IsEmpty(results.Errors);
            return results.CompiledAssembly;
        }
    }
}
