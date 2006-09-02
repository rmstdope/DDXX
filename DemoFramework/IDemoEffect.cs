using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoEffect : IRegisterable
    {
        void Step();

        void Render();

        void Initialize();
    }
}
