using System;
using System.Text;
using System.Collections.Generic;

namespace Dope.DDXX.Utility
{
    public class Interpolator<Type>
        where Type : IArithmetic
    {
        List<ISpline<Type>> splines = new List<ISpline<Type>>();

        public void AddSpline(ISpline<Type> spline)
        {
            foreach (ISpline<Type> cmpSpline in splines)
            {
                if (spline.StartTime < cmpSpline.EndTime &&
                    spline.EndTime > cmpSpline.StartTime)
                    throw new DDXXException("Overlapping splines can not exist in the same interpolator.");
            }
            splines.Add(spline);
            splines.Sort(delegate(ISpline<Type> spline1, ISpline<Type> spline2)
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


        public Type GetValue(float time)
        {
            int i = 0;
            while (i < splines.Count - 1 &&
                splines[i].EndTime < time)
                i++;
            return splines[i].GetValue(time);
        }
    }
}
