namespace MonoKle
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Class representing a serializable grid with accompanying operations.
    /// </summary>
    [Serializable()]
    public class MGrid
    {
        private float cellSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MGrid"/> class.
        /// </summary>
        /// <param name="cellSize">Size of the cells.</param>
        public MGrid(float cellSize)
        {
            this.cellSize = cellSize;
        }

        /// <summary>
        /// Gets the size of the cells.
        /// </summary>
        /// <value>
        /// The size of the cells.
        /// </value>
        public float CellSize => cellSize;

        /// <summary>
        /// Returns the cell containing the provided point.
        /// </summary>
        /// <param name="point">The provided point.</param>
        /// <returns>Cell containing the provided point.</returns>
        public MPoint2 CellFromPoint(MVector2 point) =>
            new MPoint2((int)(point.X / this.cellSize) + (point.X > 0 ? 0 : -1),
                (int)(point.Y / this.cellSize) + (point.Y > 0 ? 0 : -1));

        /// <summary>
        /// Returns the bounding rectangle of the provided cell.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>Bounding area.</returns>
        public MRectangle CellRectangle(MPoint2 cell) => new MRectangle(cell.X * this.cellSize, cell.Y * this.cellSize, this.cellSize, this.cellSize);

        /// <summary>
        /// Returns all the cells containing the provided circle.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <returns>List of cells.</returns>
        public List<MPoint2> CellsFromCircle(MCircle circle)
        {
            List<MPoint2> cellList = new List<MPoint2>();
            MRectangleInt circleBox = new MRectangleInt(
                this.CellFromPoint(new MVector2(circle.Origin.X - circle.Radius, circle.Origin.Y - circle.Radius)),
                this.CellFromPoint(new MVector2(circle.Origin.X + circle.Radius, circle.Origin.Y + circle.Radius))
                );

            for (int x = circleBox.Left; x <= circleBox.Right; x++)
            {
                for (int y = circleBox.Top; y <= circleBox.Bottom; y++)
                {
                    MPoint2 point = new MPoint2(x, y);
                    if (circle.Intersects(this.CellRectangle(point)))
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
        public List<MPoint2> CellsFromLine(MVector2 lineStart, MVector2 lineEnd) => this.TraverseLine(lineStart, lineEnd).ToList();

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
            private MGrid traverser;

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
            public IEnumerator<MPoint2> GetEnumerator()
            {
                return new LineEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// Enumerator
            /// </summary>
            public class LineEnumerator : IEnumerator<MPoint2>
            {
                private int currentX;
                private int currentY;
                private float dx;
                private float dy;
                private LineEnumerable e;
                private MPoint2 endPoint;
                private bool first;
                private bool over;
                private int stepX;
                private int stepY;
                private float tDeltaX;
                private float tDeltaY;
                private float tMaxX;
                private float tMaxY;

                internal LineEnumerator(LineEnumerable e)
                {
                    this.e = e;

                    this.dx = e.end.X - e.start.X;
                    this.stepX = dx > 0 ? 1 : -1;
                    this.tDeltaX = e.traverser.cellSize / dx;

                    this.dy = e.end.Y - e.start.Y;
                    this.stepY = dy > 0 ? 1 : -1;
                    this.tDeltaY = e.traverser.cellSize / dy;

                    this.endPoint = new MPoint2(this.e.end / this.e.traverser.cellSize);

                    this.Reset();
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                public MPoint2 Current
                {
                    get { return new MPoint2(this.currentX, this.currentY); }
                }

                object IEnumerator.Current
                {
                    get { return this.Current; }
                }

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
                    if (this.first)
                    {
                        this.first = false;
                    }
                    else
                    {
                        if (Math.Abs(tMaxX) < Math.Abs(tMaxY))
                        {
                            tMaxX = tMaxX + tDeltaX;
                            this.currentX += stepX;
                        }
                        else
                        {
                            tMaxY = tMaxY + tDeltaY;
                            this.currentY += stepY;
                        }
                    }

                    if (this.over == false && this.currentX == this.endPoint.X && this.currentY == this.endPoint.Y)
                    {
                        this.over = true;
                        return true;
                    }
                    return !this.over;
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                public void Reset()
                {
                    if (this.stepX >= 0)
                    {
                        this.tMaxX = this.tDeltaX * (1.0f - this.Frac(e.start.X / e.traverser.cellSize));
                    }
                    else
                    {
                        this.tMaxX = this.tDeltaX * (this.Frac(e.start.X / e.traverser.cellSize));
                    }
                    if (this.stepY >= 0)
                    {
                        this.tMaxY = this.tDeltaY * (1.0f - this.Frac(e.start.Y / e.traverser.cellSize));
                    }
                    else
                    {
                        this.tMaxY = this.tDeltaY * (this.Frac(e.start.Y / e.traverser.cellSize));
                    }
                    this.currentX = (int)(this.e.start.X / this.e.traverser.cellSize);
                    this.currentY = (int)(this.e.start.Y / this.e.traverser.cellSize);
                    this.first = true;
                    this.over = false;
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private float Frac(float value)
                {
                    return value - (int)value;
                }
            }
        }
    }
}