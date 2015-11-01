namespace MonoKle.Utilities
{
    using Microsoft.Xna.Framework;
    using MonoKle.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper class for traversing grids between arbitrary points, providing the tiles traversed inbetween.
    /// </summary>
    /// <remarks>
    /// Based on the paper by Woo & Amanatides (1987) http://www.cse.yorku.ca/~amana/research/grid.pdf
    /// </remarks>
    public class GridTraverser
    {
        private float cellSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridTraverser"/> class.
        /// </summary>
        /// <param name="cellSize">Size of the cells.</param>
        public GridTraverser(float cellSize)
        {
            this.cellSize = cellSize;
        }

        /// <summary>
        /// Traverses all the cells and returns a list of them.
        /// </summary>
        /// <param name="start">The start coordinate.</param>
        /// <param name="end">The end coordinate.</param>
        /// <returns>List of traversed cells.</returns>
        public List<IntVector2> TraverseAll(Vector2 start, Vector2 end)
        {
            return this.TraverseIteratively(start, end).ToList();
        }

        /// <summary>
        /// Traverses iteratively by returning an enumerable type.
        /// </summary>
        /// <param name="start">The start coordinate.</param>
        /// <param name="end">The end coordinate.</param>
        /// <returns>Enumerable type.</returns>
        public Enumerable TraverseIteratively(Vector2 start, Vector2 end)
        {
            return new Enumerable(this, start, end);
        }

        /// <summary>
        /// Enumerable
        /// </summary>
        public class Enumerable : IEnumerable<IntVector2>
        {
            private GridTraverser traverser;
            private Vector2 start;
            private Vector2 end;

            internal Enumerable(GridTraverser traverser, Vector2 start, Vector2 end)
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
            public IEnumerator<IntVector2> GetEnumerator()
            {
                return new Enumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// Enumerator
            /// </summary>
            public class Enumerator : IEnumerator<IntVector2>
            {
                private Enumerable e;

                private float dx;
                private float tDeltaX;
                private float tMaxX;

                private float dy;
                private float tDeltaY;
                private float tMaxY;

                private int stepX;
                private int stepY;

                private int currentX;
                private int currentY;

                private IntVector2 endPoint;

                private bool first;
                private bool over;

                internal Enumerator(Enumerable e)
                {
                    this.e = e;

                    this.dx = e.end.X - e.start.X;
                    this.stepX = dx > 0 ? 1 : -1;
                    this.tDeltaX = e.traverser.cellSize / dx;

                    this.dy = e.end.Y - e.start.Y;
                    this.stepY = dy > 0 ? 1 : -1;
                    this.tDeltaY = e.traverser.cellSize / dy;

                    this.endPoint = new IntVector2(this.e.end / this.e.traverser.cellSize);

                    this.Reset();
                }

                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                public IntVector2 Current
                {
                    get { return new IntVector2(this.currentX, this.currentY); }
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
                        this.tMaxX = this.tDeltaX * (1.0f - Frac(e.start.X / e.traverser.cellSize));
                    }
                    else
                    {
                        this.tMaxX = this.tDeltaX * (Frac(e.start.X / e.traverser.cellSize));
                    }
                    if (this.stepY >= 0)
                    {
                        this.tMaxY = this.tDeltaY * (1.0f - Frac(e.start.Y / e.traverser.cellSize));
                    }
                    else
                    {
                        this.tMaxY = this.tDeltaY * (Frac(e.start.Y / e.traverser.cellSize));
                    }
                    this.currentX = (int)(this.e.start.X / this.e.traverser.cellSize);
                    this.currentY = (int)(this.e.start.Y / this.e.traverser.cellSize);
                    this.first = true;
                    this.over = false;
                }

                object IEnumerator.Current
                {
                    get { return this.Current; }
                }

                private float Frac(float value)
                {
                    return value - (int)value;
                }
            }
        }
    }
}