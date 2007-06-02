using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    public interface ISystemParticleSpawner
    {
        SystemParticle Spawn();
        Type VertexType { get; }
        VertexDeclaration VertexDeclaration { get; }
        int NumInitialSpawns { get; }
        int MaxNumParticles { get; }
    }
}
