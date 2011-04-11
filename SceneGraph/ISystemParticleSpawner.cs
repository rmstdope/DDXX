using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface ISystemParticleSpawner
    {
        MaterialHandler MaterialHandler { get; }
        BlendFunction BlendFunction { get; }
        Blend SourceBlend { get; }
        Blend DestinationBlend { get; }
        ISystemParticle<float> Spawn();
        Type VertexType { get; }
        VertexDeclaration VertexDeclaration { get; }
        int NumInitialSpawns { get; }
        int MaxNumParticles { get; }
        bool ShouldSpawn();
        string GetTechniqueName(bool textured);
        Array CreateVertexArray();
    }
}
