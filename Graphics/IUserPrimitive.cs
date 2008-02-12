using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Graphics
{
    public interface IUserPrimitive<T> 
        where T:struct
    {
        void Begin();
        void End();
        void AddVertex(T vertex);
    }
}
