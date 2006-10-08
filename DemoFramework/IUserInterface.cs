using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface IUserInterface
    {
        void Initialize();
        void DrawControl(BaseControl control);
    }
}
