using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DreamBuildPlay2009
{
    public class Clipmap
    {
        // The distance between two horizontal/vertical vertices
        private int g;
        // The levelindex of current clip
        private int l;
        // Gets framesize in number of vertices between outer border of current clip
        // and outer border of next finer (inner) clip.
        private int m;
        // Gets the width of a clip in number of vertices.
        private int n;

        private Rectangle clipRegion;
        private const float heightScaling = 32;
        private TerrainVertex[] vertices;
        private short[] indices;
        private IVertexDeclaration vertexDeclaration;
        private int stripIndex = 0;
        private int NumTriangles
        {
            get { return stripIndex >= 3 ? this.stripIndex - 2 : 0; }
        }
        private float[] heightfield;
        private int heightFieldSize;
        private IGraphicsFactory graphicsFactory;

        public Clipmap(int l, int n, ref float[] heightfield, IGraphicsFactory graphicsFactory)
        {
            this.l = l;
            this.n = n;
            this.graphicsFactory = graphicsFactory;
            this.heightfield = heightfield;
            this.heightFieldSize = (int)Math.Sqrt(heightfield.Length);
            this.g = (int)Math.Pow(2, l);
            this.m = (n + 1) / 4;
            this.clipRegion = new Rectangle(0, 0, (n - 1) * g, (n - 1) * g);
            InitializeVertices();
        }

        private void InitializeVertices()
        {
            vertexDeclaration = graphicsFactory.CreateVertexDeclaration(TerrainVertex.VertexElements);
            vertices = new TerrainVertex[n * n];
            for (int z = 0; z < n; z++)
            {
                for (int x = 0; x < n; x++)
                {
                    UpdateVertex(x * g, z * g);
                }
            }

            indices = new short[4 * (3 * m * m + (n * n) / 2 + 4 * m - 10)];
            for (int z = 0; z < n - 1; z++)
            {
                FillRow(0, n - 1, z, z + 1);
            }
        }

        public void UpdateVertices(Vector3 center)
        {
            UpdateVertices((int)center.X, (int)center.Z);
        }

        private void UpdateVertices(int cx, int cz)
        {
            // Store the old position to be able to recover it if needed
            int oldX = clipRegion.X;
            int oldZ = clipRegion.Y;

            // Calculate the new position
            clipRegion.X = cx - ((n + 1) * g / 2);
            clipRegion.Y = cz - ((n + 1) * g / 2);

            // Calculate the modulo to G * 2 of the new position.
            // This makes sure that the current level always fits in the hole of the
            // coarser level. The gridspacing of the coarser level is G * 2.
            int modX = clipRegion.X % (g * 2);
            int modY = clipRegion.Y % (g * 2);
            modX += modX < 0 ? (g * 2) : 0;
            modY += modY < 0 ? (g * 2) : 0;
            clipRegion.X += (g * 2) - modX;
            clipRegion.Y += (g * 2) - modY;

            // Calculate the moving distance
            int dx = (clipRegion.X - oldX);
            int dz = (clipRegion.Y - oldZ);

            // Create some better readable variables.
            int xmin = clipRegion.Left;
            int xmax = clipRegion.Right;
            int zmin = clipRegion.Top;
            int zmax = clipRegion.Bottom;

            // Update L region
            if (dz > 0)
            {
                // Center moved in positive z direction.

                for (int z = zmin; z <= zmax - dz; z += g)
                {
                    if (dx > 0)
                    {
                        // Center moved in positive x direction.
                        // Update the right part of the L shaped region.
                        for (int x = xmax - dx + g; x <= xmax; x += g)
                        {
                            UpdateVertex(x, z);
                        }
                    }
                    else if (dx < 0)
                    {
                        // Center moved in negative x direction.
                        // Update the left part of the L shaped region.
                        for (int x = xmin; x <= xmin - dx - g; x += g)
                        {
                            UpdateVertex(x, z);
                        }
                    }
                }

                for (int z = zmax - dz + g; z <= zmax; z += g)
                {
                    // Update the bottom part of the L shaped region.
                    for (int x = xmin; x <= xmax; x += g)
                    {
                        UpdateVertex(x, z);
                    }
                }
            }
            else
            {
                // Center moved in negative z direction.

                for (int z = zmin; z <= zmin - dz - g; z += g)
                {
                    // Update the top part of the L shaped region.
                    for (int x = xmin; x <= xmax; x += g)
                    {
                        UpdateVertex(x, z);
                    }
                }

                for (int z = zmin - dz; z <= zmax; z += g)
                {
                    if (dx > 0)
                    {
                        // Center moved in poistive x direction.
                        // Update the right part of the L shaped region.
                        for (int x = xmax - dx + g; x <= xmax; x += g)
                        {
                            UpdateVertex(x, z);
                        }
                    }
                    else if (dx < 0)
                    {
                        // Center moved in negative x direction.
                        // Update the left part of the L shaped region.
                        for (int x = xmin; x <= xmin - dx - g; x += g)
                        {
                            UpdateVertex(x, z);
                        }
                    }
                }
            }
        }

        private void UpdateVertex(int x, int z)
        {
            int posx = (x / g) % n;
            int posy = (z / g) % n;
            posx += posx < 0 ? n : 0;
            posy += posy < 0 ? n : 0;

            int index = posx + posy * n;
            vertices[index].Position = new Vector4(x, 0, z, 0);
            if (x > 0 && x < heightFieldSize - 1 &&
                z > 0 && z < heightFieldSize - 1)
            {
                int k = x + z * heightFieldSize;
                int j;
                int l;
                if ((x % (g * 2)) == 0)
                {
                    if ((z % (g * 2)) == 0)
                    {
                        // Coordinates are regular. Dont need additional heightvalue.
                        j = k;
                        l = k;
                    }
                    else
                    {
                        // Z value is not regular. Get indices from higher and lover vertex.
                        j = x + (z - g) * heightFieldSize;
                        l = x + (z + g) * heightFieldSize;
                    }
                }
                else
                {
                    if ((z % (g * 2)) == 0)
                    {
                        // Z value is not regular. Get indices from higher and lover vertex.
                        j = (x - g) + z * heightFieldSize;
                        l = (x + g) + z * heightFieldSize;
                    }
                    else
                    {
                        // X value is not regular. Get indices from left and right vertex.
                        j = (x - g) + (z + g) * heightFieldSize;
                        l = (x + g) + (z - g) * heightFieldSize;
                    }
                }

                // Get the height of current coordinates
                // and set both heightvalues to that height.
                float height = heightfield[k] * heightScaling;
                vertices[index].Position.Y = height;
                vertices[index].Position.W = height;

                if (l >= 0 && l < heightfield.Length && j >= 0 && j < heightfield.Length)
                {
                    // If we can get the additional height, get the two values, and apply the
                    // median of it to the W value
                    float coarser1 = heightfield[j] * heightScaling;
                    float coarser2 = heightfield[l] * heightScaling;
                    vertices[index].Position.W = (coarser2 + coarser1) * 0.5f;
                }
            }
        }

        private BoundingFrustum frustum;
        private BoundingBox box;

        public void UpdateIndices(Clipmap nextFinerLevel, BoundingFrustum frustum)
        {
            this.frustum = frustum;

            stripIndex = 0;

            #region Fill MxM Blocks
            // MxM Block 1
            Fill_Block(clipRegion.Left,
                       clipRegion.Left + (m - 1) * g,
                       clipRegion.Top,
                       clipRegion.Top + (m - 1) * g);

            // MxM Block 2
            Fill_Block(clipRegion.Left + (m - 1) * g,
                       clipRegion.Left + 2 * ((m - 1) * g),
                       clipRegion.Top,
                       clipRegion.Top + (m - 1) * g);

            // MxM Block 3
            Fill_Block(clipRegion.Right - 2 * ((m - 1) * g),
                       clipRegion.Right - (m - 1) * g,
                       clipRegion.Top,
                       clipRegion.Top + (m - 1) * g);

            // MxM Block 4
            Fill_Block(clipRegion.Right - (m - 1) * g,
                       clipRegion.Right,
                       clipRegion.Top,
                       clipRegion.Top + (m - 1) * g);

            // MxM Block 5
            Fill_Block(clipRegion.Left,
                       clipRegion.Left + (m - 1) * g,
                       clipRegion.Top + (m - 1) * g,
                       clipRegion.Top + 2 * ((m - 1) * g));

            // MxM Block 6
            Fill_Block(clipRegion.Right - (m - 1) * g,
                       clipRegion.Right,
                       clipRegion.Top + (m - 1) * g,
                       clipRegion.Top + 2 * ((m - 1) * g));

            // MxM Block 7
            Fill_Block(clipRegion.Left,
                       clipRegion.Left + (m - 1) * g,
                       clipRegion.Bottom - 2 * ((m - 1) * g),
                       clipRegion.Bottom - (m - 1) * g);

            // MxM Block 8
            Fill_Block(clipRegion.Right - (m - 1) * g,
                       clipRegion.Right,
                       clipRegion.Bottom - 2 * ((m - 1) * g),
                       clipRegion.Bottom - (m - 1) * g);

            // MxM Block 9
            Fill_Block(clipRegion.Left,
                       clipRegion.Left + (m - 1) * g,
                       clipRegion.Bottom - (m - 1) * g,
                       clipRegion.Bottom);

            // MxM Block 10
            Fill_Block(clipRegion.Left + (m - 1) * g,
                       clipRegion.Left + 2 * ((m - 1) * g),
                       clipRegion.Bottom - (m - 1) * g,
                       clipRegion.Bottom);

            // MxM Block 11
            Fill_Block(clipRegion.Right - 2 * ((m - 1) * g),
                       clipRegion.Right - (m - 1) * g,
                       clipRegion.Bottom - (m - 1) * g,
                       clipRegion.Bottom);

            // MxM Block 12
            Fill_Block(clipRegion.Right - (m - 1) * g,
                       clipRegion.Right,
                       clipRegion.Bottom - (m - 1) * g,
                       clipRegion.Bottom);
            #endregion

            #region Fill Fixup Blocks
            // Fixup Top 
            Fill_Block(clipRegion.Left + 2 * (m - 1) * g,
                       clipRegion.Left + 2 * (m - 1) * g + (g * 2),
                       clipRegion.Top,
                       clipRegion.Top + (m - 1) * g);

            // Fixup Left
            Fill_Block(clipRegion.Left,
                       clipRegion.Left + (m - 1) * g,
                       clipRegion.Top + 2 * (m - 1) * g,
                       clipRegion.Top + 2 * (m - 1) * g + (g * 2));

            // Fixup Right
            Fill_Block(clipRegion.Right - (m - 1) * g,
                       clipRegion.Right,
                       clipRegion.Top + 2 * (m - 1) * g,
                       clipRegion.Top + 2 * (m - 1) * g + (g * 2));

            // Fixup Bottom
            Fill_Block(clipRegion.Left + 2 * (m - 1) * g,
                       clipRegion.Left + 2 * (m - 1) * g + (g * 2),
                       clipRegion.Bottom - (m - 1) * g,
                       clipRegion.Bottom);

            if (nextFinerLevel != null)
            {
                if ((nextFinerLevel.clipRegion.X - clipRegion.X) / g == m)
                {
                    if ((nextFinerLevel.clipRegion.Y - clipRegion.Y) / g == m)
                    {
                        // Upper Left L Shape

                        // Up
                        Fill_Block(clipRegion.Left + (m - 1) * g,
                                   clipRegion.Right - (m - 1) * g,
                                   clipRegion.Top + (m - 1) * g,
                                   clipRegion.Top + (m - 1) * g + g);
                        // Left
                        Fill_Block(clipRegion.Left + (m - 1) * g,
                                   clipRegion.Left + (m - 1) * g + g,
                                   clipRegion.Top + (m - 1) * g + g,
                                   clipRegion.Bottom - (m - 1) * g);
                    }
                    else
                    {
                        // Lower Left L Shape

                        // Left
                        Fill_Block(clipRegion.Left + (m - 1) * g,
                                   clipRegion.Left + (m - 1) * g + g,
                                   clipRegion.Top + (m - 1) * g,
                                   clipRegion.Bottom - (m - 1) * g - g);

                        // Bottom
                        Fill_Block(clipRegion.Left + (m - 1) * g,
                                   clipRegion.Right - (m - 1) * g,
                                   clipRegion.Bottom - (m - 1) * g - g,
                                   clipRegion.Bottom - (m - 1) * g);
                    }
                }
                else
                {
                    if ((nextFinerLevel.clipRegion.Y - clipRegion.Y) / g == m)
                    {
                        // Upper Right L Shape

                        // Up
                        Fill_Block(clipRegion.Left + (m - 1) * g,
                                   clipRegion.Right - (m - 1) * g,
                                   clipRegion.Top + (m - 1) * g,
                                   clipRegion.Top + (m - 1) * g + g);
                        // Right
                        Fill_Block(clipRegion.Right - (m - 1) * g - g,
                                   clipRegion.Right - (m - 1) * g,
                                   clipRegion.Top + (m - 1) * g + g,
                                   clipRegion.Bottom - (m - 1) * g);
                    }
                    else
                    {
                        // Lower Right L Shape

                        // Right
                        Fill_Block(clipRegion.Right - (m - 1) * g - g,
                                   clipRegion.Right - (m - 1) * g,
                                   clipRegion.Top + (m - 1) * g,
                                   clipRegion.Bottom - (m - 1) * g - g);

                        // Bottom
                        Fill_Block(clipRegion.Left + (m - 1) * g,
                                   clipRegion.Right - (m - 1) * g,
                                   clipRegion.Bottom - (m - 1) * g - g,
                                   clipRegion.Bottom - (m - 1) * g);
                    }
                }
            }
            #endregion

            #region Fill Fine Inner Level
            if (nextFinerLevel == null)
            {
                Fill_Block(clipRegion.Left + (m - 1) * g,
                           clipRegion.Left + (m - 1) * g + n / 2,
                           clipRegion.Top + (m - 1) * g,
                           clipRegion.Top + (m - 1) * g + n / 2);

                Fill_Block(clipRegion.Left + (m - 1) * g + n / 2,
                           clipRegion.Right - (m - 1) * g,
                           clipRegion.Top + (m - 1) * g,
                           clipRegion.Top + (m - 1) * g + n / 2);

                Fill_Block(clipRegion.Left + (m - 1) * g,
                           clipRegion.Left + (m - 1) * g + n / 2,
                           clipRegion.Top + (m - 1) * g + n / 2,
                           clipRegion.Bottom - (m - 1) * g);

                Fill_Block(clipRegion.Left + (m - 1) * g + n / 2,
                           clipRegion.Right - (m - 1) * g,
                           clipRegion.Top + (m - 1) * g + n / 2,
                           clipRegion.Bottom - (m - 1) * g);
            }
            #endregion
        }

        private void Fill_Block(int left, int right, int top, int bot)
        {
            // Setup the boundingbox of the block to fill.
            // The lowest value is zero, the highest is the scalesize.
            box.Min.X = left;
            box.Min.Y = 0;
            box.Min.Z = top;
            box.Max.X = right;
            box.Max.Y = heightScaling;
            box.Max.Z = bot;

            if (frustum.Contains(box) != ContainmentType.Disjoint)
            {
                // Same moduloprocedure as when we updated the vertices.
                // Maps the terrainposition to arrayposition.
                left = (left / g) % n;
                right = (right / g) % n;
                top = (top / g) % n;
                bot = (bot / g) % n;
                left += left < 0 ? n : 0;
                right += right < 0 ? n : 0;
                top += top < 0 ? n : 0;
                bot += bot < 0 ? n : 0;

                // Now fill the block.
                if (bot < top)
                {
                    // Bottom border is positioned somwhere over the top border,
                    // we have a wrapover so we must split up the update in two parts.

                    // Go from top border to the end of the array and update every row
                    for (int z = top; z <= n - 2; z++)
                    {
                        FillRow(left, right, z, z + 1);
                    }

                    // Update the wrapover row
                    FillRow(left, right, n - 1, 0);

                    // Go from arraystart to the bottom border and update every row.
                    for (int z = 0; z <= bot - 1; z++)
                    {
                        FillRow(left, right, z, z + 1);
                    }
                }
                else
                {
                    // Top boarder is over the bottom boarder. Update from top to bottom.
                    for (int z = top; z <= bot - 1; z++)
                    {
                        FillRow(left, right, z, z + 1);
                    }
                }
            }
        }

        private void FillRow(int x0, int xn, int zn, int zn1)
        {
            // Rows are made of trianglestrips. All rows together build up a big single
            // trianglestrip. The probloem is when a row ends, it spans a triangle to the
            // start of the next row. We must hide that triangles. Therefore we add two
            // dummy indices, Twice the starting index and twice the ending index. This
            // will result in invisible triangles because when a triangle has two vertices
            // that are at exactly the same place, there is no area that the triangle can
            // cover. So four triangles between two rows look like this: 
            // (prev, END, END) (END, END, START') (END, START', START') and (START', START', next)
            // so we have four invisible triangles but all rows in a single trianglestrip.

            addIndex(x0, zn); // "START" dummyindex
            if (x0 <= xn)
            {
                for (int x = x0; x <= xn; x++)
                {
                    addIndex(x, zn);
                    addIndex(x, zn1);
                }
            }
            else
            {
                for (int x = x0; x <= n - 1; x++)
                {
                    addIndex(x, zn);
                    addIndex(x, zn1);
                }
                for (int x = 0; x <= xn; x++)
                {
                    addIndex(x, zn);
                    addIndex(x, zn1);
                }
            }
            addIndex(xn, zn1); // "END" dummyindex
        }

        private void addIndex(int x, int z)
        {
            // calculate the index
            int i = x + z * n;
            // add the index and increment counter.
            indices[stripIndex++] = (short)i;
        }

    }
}
