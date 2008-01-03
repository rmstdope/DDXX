using System;

namespace Dope.DDXX.Sound
{
    public interface ISoundDriver
    {
        void Initialize(string resourceName);
        ICue PlaySound(string name);
        void Step();
    }
}
