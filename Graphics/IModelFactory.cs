using System;
namespace Dope.DDXX.Graphics
{
    public enum ModelOptions
    {
        None = 0,
        NoOptimization = 1,
        EnsureTangents = 2
        // Continue with 4, 8, 16, 32, etc.
    }

    public interface IModelFactory
    {
        int Count { get; }
        int CountBoxes { get; }
        int CountFiles { get; }
        IModel CreateBox(float width, float height, float depth);
        IModel FromFile(string file, ModelOptions options);
    }
}
