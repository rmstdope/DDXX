using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
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
using Microsoft.DirectX;
public class FooEffect : TweakableContainer, IDemoEffect 
{
  protected float start; protected float end;
  public FooEffect(float f1, float f2) { start = f1; end = f2;}
  public void Step() {} public void Render() {} public void Initialize(IGraphicsFactory graphicsFactory, IEffectFactory effectFactory, IDevice device, IDemoMixer mixer) {} 
  public float StartTime { get { return start;} set { start = value;} }
  public float EndTime { get { return end;} set { end = value;} }
}
public class BarEffect : FooEffect {
  private int intParam;
  private float floatParam;
  private Vector3 vector3Param;
  public BarEffect(float start, float end) : base(start, end) { 
    this.start = start;
    this.end = end; 
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
            IRegisterable ei = effectTypes.CreateInstance("fooeffect", 0, 1);
            Assert.IsNull(ei);
        }

        [Test]
        public void TestCreateInstanceOK1()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("FooEffect", 3, 7);
            Assert.IsNotNull(ei);
            Assert.IsInstanceOfType(assembly.GetType("FooEffect"), ei);
            Assert.AreEqual(3, ei.StartTime);
            Assert.AreEqual(7, ei.EndTime);
        }

        [Test]
        public void TestCreateInstanceOK2()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", 3, 7);
            Assert.IsNotNull(ei);
            Assert.IsInstanceOfType(assembly.GetType("BarEffect"), ei);
            Assert.AreEqual(3, ei.StartTime);
            Assert.AreEqual(7, ei.EndTime);
        }

        [Test]
        public void TestSetIntParameter()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", 0, 1);
            Assert.IsNotNull(ei);
            effectTypes.SetProperty(ei, "IntParam", 5);
            int v = (int)effectTypes.GetProperty(ei, "IntParam");
            Assert.AreEqual(6, v);
        }

        [Test]
        public void TestSetFloatParameter()
        {
            effectTypes = new DemoEffectTypes(new Assembly[] { assembly });
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", 0, 1);
            Assert.IsNotNull(ei);
            effectTypes.SetProperty(ei, "FloatParam", 5.5F);
            float v = (float)effectTypes.GetProperty(ei, "FloatParam");
            Assert.AreEqual(11.1F, v);
        }

        private Assembly SetupAssembly(string source)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //string windir = (string)Registry.GetIntValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Windows", "Directory", "");
            string[] sysdir = Environment.SystemDirectory.Split('\\');
            string assemblyDir = string.Join("\\", sysdir, 0, sysdir.Length - 1) + "\\assembly\\";
            AssemblyName[] referenced = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            string d3dAssembly = Assembly.GetAssembly(typeof(Microsoft.DirectX.Vector3)).CodeBase;
            d3dAssembly = d3dAssembly.Remove(0, 8);
            //string d3dAssembly = assemblyDir + "Microsoft.DirectX.Direct3D.dll";
            CompilerParameters cp = new CompilerParameters(new string[] { "Dope.DDXX.DemoFramework.dll", "Dope.DDXX.Graphics.dll", d3dAssembly });
            results = provider.CompileAssemblyFromSource(cp, source);
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError e in results.Errors)
                    Console.WriteLine(e.ToString());
                Assert.Fail("Internal error in test code");
            }
            Assert.IsEmpty(results.Errors);
            return results.CompiledAssembly;
        }
    }
}
