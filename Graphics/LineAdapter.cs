using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public class LineAdapter : ILine
    {
        private Line line;

        public LineAdapter(Line line)
        {
            this.line = line;
        }

        #region ILine Members

        public bool Antialias
        {
            get
            {
                return line.Antialias;
            }
            set
            {
                line.Antialias = value;
            }
        }

        public bool Disposed
        {
            get { return line.Disposed; }
        }

        public bool GlLines
        {
            get
            {
                return line.GlLines;
            }
            set
            {
                line.GlLines = value;
            }
        }

        public int Pattern
        {
            get
            {
                return line.Pattern;
            }
            set
            {
                line.Pattern = value;
            }
        }

        public float PatternScale
        {
            get
            {
                return line.PatternScale;
            }
            set
            {
                line.PatternScale= value;
            }
        }

        public float Width
        {
            get
            {
                return line.Width;
            }
            set
            {
                line.Width = value;
            }
        }

        public void Begin()
        {
            line.Begin();
        }

        public void Draw(Vector2[] vertexList, Color color)
        {
            line.Draw(vertexList, color);
        }

        public void Draw(Vector2[] vertexList, int color)
        {
            line.Draw(vertexList, color);
        }

        public void DrawTransform(Vector3[] vertexList, Matrix transform, Color color)
        {
            line.DrawTransform(vertexList, transform, color);
        }

        public void DrawTransform(Vector3[] vertexList, Matrix transform, int color)
        {
            line.DrawTransform(vertexList, transform, color);
        }

        public void End()
        {
            line.End();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            line.Dispose();
        }

        #endregion
    }
}
