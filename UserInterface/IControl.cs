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
        float GetHeight(IDrawResources resources);
        float GetWidth(IDrawResources resources);
        float GetX1(IDrawResources resources);
        float GetY1(IDrawResources resources);
        void RemoveChild(IControl control);
    }
}
