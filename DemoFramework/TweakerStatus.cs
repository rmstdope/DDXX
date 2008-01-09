using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public class TweakerStatus
    {
        private int selection;
        private BaseControl rootControl;
        private float timeScale;
        private float startTime;
        private float variableSpacing;
        private int index;
        private string inputString;

        public int Selection
        {
            get { return selection; }
            set 
            {
                if (value != selection)
                    index = 0;
                selection = value; 
            }
        }

        public BaseControl RootControl
        {
            get { return rootControl; }
            set { rootControl = value; }
        }

        public float TimeScale
        {
            get { return timeScale; }
        }

        public float VariableSpacing
        {
            get { return variableSpacing; }
        }

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string InputString
        {
            get { return inputString; }
            set { inputString = value; }
        }

        public TweakerStatus(float timeScale, float variableSpacing)
        {
            selection = 0;
            this.timeScale = timeScale;
            this.variableSpacing = variableSpacing;
        }
    }
}
