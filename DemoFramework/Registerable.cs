using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public class Registerable : IRegisterable
    {
        private string name;
        private float startTime;
        private float endTime;

        public Registerable(string name, float startTime, float endTime)
        {
            this.name = name;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public string Name
        {
            get { return name; }
        }

        public float StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }

        public float EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
            }
        }
    }
}
