using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.Win32;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoEffectTypesTest
    {
        string source = @"
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX;
public class FooEffect : IDemoEffect 
{
  protected float start; protected float end;
  public void Step() {} public void Render() {} public void Initialize() {} 
  public float StartTime { get { return start;} set { start = value;} }
  public float EndTime { get { return end;} set { end = value;} }
}
public class BarEffect : FooEffect {
  private int intParam;
  private float floatParam;
  private Vector3 vector3Param;
  public BarEffect() : base() {}
  public BarEffect(float start, float end) { 
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
            SetupAssembly(source);
            effectTypes = new DemoEffectTypes();
            effectTypes.Initialize(assembly);
        }

        [TearDown]
        public void TearDown()
        {
            results.TempFiles.Delete();
        }

        [Test]
        public void TestFindEffectsInAssembly()
        {
            Assert.AreEqual(2, effectTypes.Types.Count);
            Type t;
            Assert.IsTrue(effectTypes.Types.TryGetValue("FooEffect", out t));
            Assert.AreEqual(assembly.GetType("FooEffect"), t);
            Assert.IsTrue(effectTypes.Types.TryGetValue("BarEffect", out t));
            Assert.AreEqual(assembly.GetType("BarEffect"), t);
        }

        [Test]
        public void TestCreateInstance()
        {
            IRegisterable ei = effectTypes.CreateInstance("fooeffect", 0, 1);
            Assert.IsNull(ei);

            ei = effectTypes.CreateInstance("FooEffect", 3, 7);
            Assert.IsNotNull(ei);
            Assert.IsInstanceOfType(assembly.GetType("FooEffect"), ei);
            Assert.AreEqual(3, ei.StartTime);
            Assert.AreEqual(7, ei.EndTime);

            ei = effectTypes.CreateInstance("BarEffect", 3, 7);
            Assert.IsNotNull(ei);
            Assert.IsInstanceOfType(assembly.GetType("BarEffect"), ei);
            Assert.AreEqual(3, ei.StartTime);
            Assert.AreEqual(7, ei.EndTime);
        }

        [Test]
        public void TestSetIntParameter()
        {
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", 0, 1);
            Assert.IsNotNull(ei);
            effectTypes.SetProperty(ei, "IntParam", 5);
            int v = (int)effectTypes.GetProperty(ei, "IntParam");
            Assert.AreEqual(6, v);
        }

        [Test]
        public void TestSetFloatParameter()
        {
            IRegisterable ei = effectTypes.CreateInstance("BarEffect", 0, 1);
            Assert.IsNotNull(ei);
            effectTypes.SetProperty(ei, "FloatParam", 5.5F);
            float v = (float)effectTypes.GetProperty(ei, "FloatParam");
            Assert.AreEqual(11.1F, v);
        }

        private void SetupAssembly(string source)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //string windir = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Windows", "Directory", "");
            string[] sysdir = Environment.SystemDirectory.Split('\\');
            string assemblyDir = string.Join("\\", sysdir, 0, sysdir.Length - 1) + "\\assembly\\";
            AssemblyName[] referenced = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            string d3dAssembly = Assembly.GetAssembly(typeof(Microsoft.DirectX.Vector3)).CodeBase;
            d3dAssembly = d3dAssembly.Remove(0, 8);
            //string d3dAssembly = assemblyDir + "Microsoft.DirectX.Direct3D.dll";
            CompilerParameters cp = new CompilerParameters(new string[] { "Dope.DDXX.DemoFramework.dll", d3dAssembly });
            results = provider.CompileAssemblyFromSource(cp, source);
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError e in results.Errors)
                    Console.WriteLine(e.ToString());
                Assert.Fail("Internal error in test code");
            }
            Assert.IsEmpty(results.Errors);
            assembly = results.CompiledAssembly;
        }
    }
}
