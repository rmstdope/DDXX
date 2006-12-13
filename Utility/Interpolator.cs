using System;
using System.Text;
using System.Collections.Generic;

namespace Dope.DDXX.Utility
{
    public class Interpolator
    {
        List<ISpline> splines = new List<ISpline>();

        public void AddSpline(ISpline spline)
        {
            foreach (ISpline cmpSpline in splines)
            {
                if (spline.StartTime < cmpSpline.EndTime &&
                    spline.EndTime > cmpSpline.StartTime)
                    throw new DDXXException("Overlapping splines can not exist in the same interpolator.");
            }
            splines.Add(spline);
            splines.Sort(delegate(ISpline spline1, ISpline spline2)
            {
                if (spline1.StartTime < spline2.StartTime)
                    return -1;
                else
                    return 1;
            });
        }

        public float StartTime
        {
            get { return splines[0].StartTime; }
        }

        public float EndTime
        {
            get { return splines[splines.Count - 1].EndTime; }
        }

    }
}
