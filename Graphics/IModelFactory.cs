using System;
namespace Dope.DDXX.Graphics
{
    //public delegate string MaterialEffectChooser(int material);
    //public delegate MaterialEffectChooser MeshEffectChooser(string meshName);

    //public class TechniqueChooser
    //{
    //    public static MaterialEffectChooser MaterialPrefix(string prefix)
    //    {
    //        return delegate(int material) { return prefix; };
    //    }
    //    public static MeshEffectChooser MeshPrefix(string prefix)
    //    {
    //        return delegate(string name) { return MaterialPrefix(prefix); };
    //    }
    //}

    public interface IModelFactory
    {
        IModel FromFile(string file, string effect);
        //IModel FromFile(string file, MaterialEffectChooser techniqueChooser);
        //IModel FromFile(string file, MeshEffectChooser techniqueChooser);

    }
}
