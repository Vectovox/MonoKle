using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MonoKle
{
    /// <summary>
    /// Class representing a serializable grid with accompanying operations.
    /// </summary>
    [Serializable]
    public class MGrid
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MGrid"/> class.
        /// </summary>
        /// <param name="cellSize">Size of the cells.</param>
        public MGrid(float cellSize) => CellSize = cellSize;

        /// <summary>
        /// Gets the size of the cells.
        /// </summary>
        /// <value>
        /// The size of the cells.
        /// </value>
        public float CellSize { get; }

        /// <summary>
        /// Returns the cell containing the provided point.
        /// </summary>
        /// <param name="point">The provided point.</param>
        /// <returns>Cell containing the provided point.</returns>
        public MPoint2 CellFromPoint(MVector2 point) =>
            new((int)(point.X / CellSize) + (point.X > 0 ? 0 : -1),
                (int)(point.Y / CellSize) + (point.Y > 0 ? 0 : -1));

        /// <summary>
        /// Returns the bounding rectangle of the provided cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>Bounding area.</returns>
        public MRectangle CellRectangle(MPoint2 cell) => new(cell.X * CellSize, cell.Y * CellSize, CellSize, CellSize);

        /// <summary>
        /// Returns all the cells containing the provided circle.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <returns>List of cells.</returns>
        public List<MPoint2> CellsFromCircle(MCircle circle)
        {
            var cellList = new List<MPoint2>();
            var circleBox = new MRectangleInt(
                CellFromPoint(new MVector2(circle.Origin.X - circle.Radius, circle.Origin.Y - circle.Radius)),
                CellFromPoint(new MVector2(circle.Origin.X + circle.Radius, circle.Origin.Y + circle.Radius))
                );

            for (int x = circleBox.Left; x <= circleBox.Right; x++)
            {
                for (int y = circleBox.Top; y <= circleBox.Bottom; y++)
                {
                    var point = new MPoint2(x, y);
                    if (circle.Intersects(CellRectangle(point)))
                    {
                        cellList.Add(point);
                    }
                }
            }

            return cellList;
        }

        /// <summary>
        /// Returns all the cells containing the provided line. Cells are in order of occurance.
        /// </summary>
        /// <param name="lineStart">The start coordinate of the line.</param>
        /// <param name="lineEnd">The end coordinate of the line.</param>
        /// <returns>List of cells.</returns>
        public List<MPoint2> CellsFromLine(MVector2 lineStart, MVector2 lineEnd) => TraverseLine(lineStart, lineEnd).ToList();

        /// <summary>
        /// Returns an <see cref="IEnumerable{MPoint2}"/> for iteratively traversing the cells containing the provided line.
        /// </summary>
        /// <param name="lineStart">The start coordinate.</param>
        /// <param name="lineEnd">The end coordinate.</param>
        /// <returns>Enumerable type.</returns>
        /// <remarks>
        /// Based on the paper by Woo and Amanatides (1987) http://www.cse.yorku.ca/~amana/research/grid.pdf
        /// </remarks>
        public IEnumerable<MPoint2> TraverseLine(MVector2 lineStart, MVector2 lineEnd) => new LineEnumerable(this, lineStart, lineEnd);

        /// <summary>
        /// Enumerable
        /// </summary>
        public class LineEnumerable : IEnumerable<MPoint2>
        {
            private Vector2 end;
            private Vector2 start;
            private readonly MGrid traverser;

            internal LineEnumerable(MGrid traverser, MVector2 start, MVector2 end)
            {
                this.traverser = traverser;
                this.start = start;
                this.end = end;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<MPoint2> GetEnumerator() => new LineEnumerator(this);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// Enumerator
            /// </summary>
            public class LineEnumerator : IEnumerator<MPoint2>
            {
                private int currentX;
                private int currentY;
                private readonly float dx;
                private readonly float dy;
                private readonly LineEnumerable e;
                private readonly MPoint2 endPoint;
                private bool first;
                private bool over;
                private readonly int stepX;
                private readonly int stepY;
                private readonly float tDeltaX;
                private readonly float tDeltaY;
                private float tMaxX;
                private float tMaxY;

                internal LineEnumerator(LineEnumerable e)
                {
                    this.e = e;

                    dx = e.end.X - e.start.X;
                    stepX = dx > 0 ? 1 : -1;
                    tDeltaX = e.traverser.CellSize / dx;

                    dy = e.end.Y - e.start.Y;
                    stepY = dy > 0 ? 1 : -1;
                    tDeltaY = e.traverser.CellSize / dy;

                    endPoint = new MPoint2(this.e.end / this.e.traverser.CellSize);

                    Reset();
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                public MPoint2 Current => new(currentX, currentY);

                object IEnumerator.Current => Current;

                /// <summary>
                /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
                /// </summary>
                public void Dispose() { }

                /// <summary>
                /// Advances the enumerator to the next element of the collection.
                /// </summary>
                /// <returns>
                /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
                /// </returns>
                public bool MoveNext()
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        if (Math.Abs(tMaxX) < Math.Abs(tMaxY))
                        {
                            tMaxX = tMaxX + tDeltaX;
                            currentX += stepX;
                        }
                        else
                        {
                            tMaxY = tMaxY + tDeltaY;
                            currentY += stepY;
                        }
                    }

                    if (over == false && currentX == endPoint.X && currentY == endPoint.Y)
                    {
                        over = true;
                        return true;
                    }
                    return !over;
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                public void Reset()
                {
                    if (stepX >= 0)
                    {
                        tMaxX = tDeltaX * (1.0f - Frac(e.start.X / e.traverser.CellSize));
                    }
                    else
                    {
                        tMaxX = tDeltaX * (Frac(e.start.X / e.traverser.CellSize));
                    }
                    if (stepY >= 0)
                    {
                        tMaxY = tDeltaY * (1.0f - Frac(e.start.Y / e.traverser.CellSize));
                    }
                    else
                    {
                        tMaxY = tDeltaY * (Frac(e.start.Y / e.traverser.CellSize));
                    }
                    currentX = (int)(e.start.X / e.traverser.CellSize);
                    currentY = (int)(e.start.Y / e.traverser.CellSize);
                    first = true;
                    over = false;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private float Frac(float value) => value - (int)value;
            }
        }
    }
}
