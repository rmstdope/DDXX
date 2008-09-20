using System;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.UserInterface
{
    public interface IControl
    {
        void AddChild(IControl control);
        void DrawControl(IDrawResources resources);
        Vector4 Rectangle { get; }
        void RemoveChildren();
        void RemoveFromParent();
    }
}
