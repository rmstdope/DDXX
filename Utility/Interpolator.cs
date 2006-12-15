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

        private ISpline<Type> FirstSpline
        { 
            get { return splines[0]; } 
        }

        private ISpline<Type> LastSpline
        {
            get { return splines[splines.Count - 1]; }
        }

        public float StartTime
        {
            get { return FirstSpline.StartTime; }
        }

        public float EndTime
        {
            get { return LastSpline.EndTime; }
        }


        public Type GetValue(float time)
        {
            return splines[GetSpline(time)].GetValue(time);
        }

        public Type GetDerivative(float time)
        {
            return splines[GetSpline(time)].GetDerivative(time);
        }

        private int GetSpline(float time)
        {
            int i = 0;
            while (i < splines.Count - 1 &&
                splines[i].EndTime < time)
                i++;
            return i;
        }

    }
}
