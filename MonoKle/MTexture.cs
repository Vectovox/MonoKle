using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoKle
{
    /// <summary>
    /// Texture asset with underlying data and methods of a texture atlasing and animation.
    /// </summary>
    public class MTexture
    {
        /// <summary>
        /// The identifier of the texture.
        /// </summary>
        public readonly string Identifier;

        /// <summary>
        /// The source path of the texture data.
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// The raw data that the <see cref="MTexture"/> maps on.
        /// </summary>
        public readonly Texture2D Data;

        /// <summary>
        /// The source rectangle mapping the texture on <see cref="Data"/>.
        /// </summary>
        public readonly MRectangleInt AtlasRectangle;

        /// <summary>
        /// The amount of atlas frame columns.
        /// </summary>
        public readonly int FrameColumns;

        /// <summary>
        /// The amount of atlas frame rows.
        /// </summary>
        public readonly int FrameRows;

        /// <summary>
        /// The frame rate, in frames per second, of the animation.
        /// </summary>
        public readonly int FrameRate;

        /// <summary>
        /// The margin between different frames in the atlas. E.g. used for bleed zones in sprite maps or animations.
        /// </summary>
        public readonly int FrameMargin;

        public MTexture(Texture2D texture, string identifier, string path)
            : this(texture, identifier, path, 1, 1) { }

        public MTexture(Texture2D texture, string identifier, string path, int frameColumns, int frameRows)
            : this(texture, identifier, path, frameColumns, frameRows, 0) { }

        public MTexture(Texture2D texture, string identifier, string path, int frameColumns, int frameRows, int frameMargin)
            : this(texture, identifier, path, frameColumns, frameRows, frameMargin, 0) { }

        public MTexture(Texture2D texture, string identifier, string path, int frameColumns, int frameRows, int frameMargin, int frameRate)
            : this(texture, identifier, path, new MRectangleInt(0, 0, texture.Width, texture.Height), frameColumns, frameRows, frameMargin, frameRate) { }

        public MTexture(Texture2D texture, string identifier, string path, MRectangleInt atlasRectangle)
            : this(texture, identifier, path, atlasRectangle, 1, 1) { }

        public MTexture(Texture2D texture, string identifier, string path, MRectangleInt atlasRectangle, int frameColumns, int frameRows)
            : this(texture, identifier, path, atlasRectangle, frameColumns, frameRows, 0) { }

        public MTexture(Texture2D texture, string identifier, string path, MRectangleInt atlasRectangle, int frameColumns, int frameRows, int frameMargin)
            : this(texture, identifier, path, atlasRectangle, frameColumns, frameRows, frameMargin, 0) { }

        public MTexture(Texture2D texture, string identifier, string path, MRectangleInt atlasRectangle, int frameColumns, int frameRows, int frameMargin, int frameRate)
        {
            if (frameColumns < 1)
            {
                throw new ArgumentException($"Invalid column amount '{frameColumns}'. Must be greater than 0.");
            }

            if (frameRows < 1)
            {
                throw new ArgumentException($"Invalid row amount '{frameRows}'. Must be greater than 0.");
            }

            if (frameRate < 0)
            {
                throw new ArgumentException($"Invalid frame rate '{frameRate}'. Must be greater or equal to 0.");
            }

            if (frameMargin < 0)
            {
                throw new ArgumentException($"Invalid margin '{frameMargin}'. Must be greater or equal to 0.");
            }

            if ((atlasRectangle.Width - frameColumns * 2 * frameMargin) % (float)frameColumns != 0)
            {
                throw new ArgumentException($"Invalid column count, margin, or width. Columns with margin must add up to the width.");
            }

            if ((atlasRectangle.Height - frameRows * 2 * frameMargin) % (float)frameRows != 0)
            {
                throw new ArgumentException($"Invalid row count, margin, or width. Rows with margin must add up to the height.");
            }

            Data = texture;
            Identifier = identifier;
            Path = path;
            AtlasRectangle = atlasRectangle;
            FrameMargin = frameMargin;
            FrameColumns = frameColumns;
            FrameRows = frameRows;
            FrameRate = frameRate;
        }

        /// <summary>
        /// Gets the animation duration animating over columns.
        /// </summary>
        public TimeSpan DurationRow => FrameRate == 0 ? TimeSpan.MaxValue : TimeSpan.FromSeconds(FrameColumns / (float)FrameRate);

        /// <summary>
        /// Gets the animation duration animating over rows.
        /// </summary>
        public TimeSpan DurationColumn => FrameRate == 0 ? TimeSpan.MaxValue : TimeSpan.FromSeconds(FrameRows / (float)FrameRate);

        /// <summary>
        /// Gets the width of a single frame once animated.
        /// </summary>
        public int FrameWidth => AtlasRectangle.Width / FrameColumns - 2 * FrameMargin;

        /// <summary>
        /// Gets the width of a single frame once animated.
        /// </summary>
        public int FrameHeight => AtlasRectangle.Height / FrameRows - 2 * FrameMargin;

        /// <summary>
        /// Returns new instance of <see cref="MTexture"/> that represents the frame at the provided row and column.
        /// Applies <see cref="FrameMargin"/> between every cell.
        /// </summary>
        /// <remarks>
        /// Wraps around when the provided row or column is outside of the atlas.
        /// </remarks>
        /// <param name="column">Zero-based index of the column to get.</param>
        /// <param name="row">Zero-based index of the row to get.</param>
        public MTexture GetCell(int column, int row) => new MTexture(Data, Identifier, Path, GetCellAtlas(column, row));

        /// <summary>
        /// Gets the atlas rectangle for the provided column and row.
        /// </summary>
        /// <param name="column">Zero-based index of the column to get.</param>
        /// <param name="row">Zero-based index of the row to get.</param>
        public MRectangleInt GetCellAtlas(int column, int row)
        {
            int wrappedColumn = column % FrameColumns;
            int wrappedRow = row % FrameRows;
            return AtlasRectangle
                .RedimensionWidth(FrameWidth)
                .RedimensionHeight(FrameHeight)
                .TranslateX(wrappedColumn * (FrameWidth + FrameMargin + FrameMargin) + FrameMargin)
                .TranslateY(wrappedRow * (FrameHeight + FrameMargin + FrameMargin) + FrameMargin);
        }

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, getting the whole provided column, only applying <see cref="FrameMargin"/> between columns.
        /// </summary>
        /// <remarks>
        /// Wraps around when the provided column is outside of the atlas.
        /// </remarks>
        /// <param name="column">Zero-based index of the column to get.</param>
        public MTexture GetColumn(int column) => new MTexture(Data, Identifier, Path, GetColumnAtlas(column));

        /// <summary>
        /// Gets the atlas rectangle for the provided column.
        /// </summary>
        /// <param name="column">Zero-based index of the column to get.</param>
        public MRectangleInt GetColumnAtlas(int column)
        {
            int wrappedColumn = column % FrameColumns;
            return AtlasRectangle
                .RedimensionWidth(FrameWidth)
                .TranslateX(wrappedColumn * (FrameWidth + FrameMargin + FrameMargin) + FrameMargin);
        }

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, getting the whole provided row, only applying <see cref="FrameMargin"/> between rows.
        /// </summary>
        /// <remarks>
        /// Wraps around when the provided row is outside of the atlas.
        /// </remarks>
        /// <param name="row">Zero-based index of the row to get.</param>
        public MTexture GetRow(int row) => new MTexture(Data, Identifier, Path, GetRowAtlas(row));

        /// <summary>
        /// Gets the atlas rectangle for the provided column.
        /// </summary>
        /// <param name="row">Zero-based index of the row to get.</param>
        public MRectangleInt GetRowAtlas(int row)
        {
            int wrappedRow = row % FrameRows;
            return AtlasRectangle
                .RedimensionHeight(FrameHeight)
                .TranslateY(wrappedRow * (FrameHeight + FrameMargin + FrameMargin) + FrameMargin);
        }

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, animated to the given row to the provided frame.
        /// </summary>
        /// <remarks>
        /// Wraps around when the provided frame is outside of the atlas.
        /// </remarks>
        /// <param name="row">Zero-based index of the row to use.</param>
        /// <param name="frame">Zero-based index of the frame to show.</param>
        public MTexture AnimateRow(int row, int frame) => new MTexture(Data, Identifier, Path, AnimateRowAtlas(row, frame));

        /// <summary>
        /// Animates the given row to the provided frame.
        /// </summary>
        /// <remarks>
        /// Wraps around when the provided frame is outside of the atlas.
        /// </remarks>
        /// <param name="row">Zero-based index of the row to use.</param>
        /// <param name="frame">Zero-based index of the frame to show.</param>
        public MRectangleInt AnimateRowAtlas(int row, int frame) => GetCellAtlas(frame, row);

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, animated to the given row to the frame at the provided elapsed time.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="row">Zero-based index of the row to use.</param>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MTexture AnimateRow(int row, TimeSpan elapsed) => new MTexture(Data, Identifier, Path, AnimateRowAtlas(row, elapsed));

        /// <summary>
        /// Animates the given row to the frame at the provided elapsed time.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="row">Zero-based index of the row to use.</param>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MRectangleInt AnimateRowAtlas(int row, TimeSpan elapsed) => AnimateRowAtlas(row, (int)(FrameRate * elapsed.TotalSeconds));

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, animated to the frame at the provided elapsed time.
        /// The whole texture makes a singular row, only applying <see cref="FrameMargin"/> between columns.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MTexture AnimateRow(TimeSpan elapsed) => new MTexture(Data, Identifier, Path, AnimateRowAtlas(elapsed));

        /// <summary>
        /// Animates to the frame at the provided elapsed time. The whole texture makes a singular row, only applying <see cref="FrameMargin"/> between columns.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MRectangleInt AnimateRowAtlas(TimeSpan elapsed) => GetColumnAtlas((int)(FrameRate * elapsed.TotalSeconds));

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, animated to the given column to the provided frame.
        /// </summary>
        /// <param name="column">Zero-based index of the column to use.</param>
        /// <param name="frame">Zero-based index of the frame to show.</param>
        /// <remarks>
        /// Wraps around when the provided frame is outside of the atlas.
        /// </remarks>
        public MTexture AnimateColumn(int column, int frame) => new MTexture(Data, Identifier, Path, AnimateColumnAtlas(column, frame));

        /// <summary>
        /// Animates the given column to the provided frame.
        /// </summary>
        /// <param name="column">Zero-based index of the column to use.</param>
        /// <param name="frame">Zero-based index of the frame to show.</param>
        /// <remarks>
        /// Wraps around when the provided frame is outside of the atlas.
        /// </remarks>
        public MRectangleInt AnimateColumnAtlas(int column, int frame) => GetCellAtlas(column, frame);

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, animated to the given column to the frame at the provided elapsed time.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="column">Zero-based index of the column to use.</param>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MTexture AnimateColumn(int column, TimeSpan elapsed) => new MTexture(Data, Identifier, Path, AnimateColumnAtlas(column, elapsed));

        /// <summary>
        /// Animates the given column to the frame at the provided elapsed time.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="column">Zero-based index of the column to use.</param>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MRectangleInt AnimateColumnAtlas(int column, TimeSpan elapsed) => AnimateColumnAtlas(column, (int)(FrameRate * elapsed.TotalSeconds));

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/>, animated to the frame at the provided elapsed time.
        /// The whole texture makes a singular column, only applying <see cref="FrameMargin"/> between rows.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MTexture AnimateColumn(TimeSpan elapsed) => new MTexture(Data, Identifier, Path, AnimateColumnAtlas(elapsed));

        /// <summary>
        /// Animates to the frame at the provided elapsed time. The whole texture makes a singular column, only applying <see cref="FrameMargin"/> between rows.
        /// </summary>
        /// <remarks>
        /// Wraps around when the frame is outside of the atlas.
        /// </remarks>
        /// <param name="elapsed">Elapsed time used to pick out the frame.</param>
        public MRectangleInt AnimateColumnAtlas(TimeSpan elapsed) => GetRowAtlas((int)(FrameRate * elapsed.TotalSeconds));

        /// <summary>
        /// Returns new instance of <see cref="MTexture"/> that represents the frame at the provided row and column.
        /// </summary>
        /// <remarks>
        /// Wraps around when the provided row or column is outside of the atlas.
        /// </remarks>
        /// <param name="column">Zero-based index of the column to get.</param>
        /// <param name="row">Zero-based index of the row to get.</param>
        public MTexture this[int column, int row] => GetCell(column, row);

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/> with the atlas rectangle set to the provided value.
        /// </summary>
        /// <param name="atlas">The atlas rectangle to use.</param>
        public MTexture WithAtlas(MRectangleInt atlas) => new MTexture(Data, Identifier, Path, atlas, FrameColumns, FrameRows, FrameRate, FrameMargin);

        /// <summary>
        /// Returns a new instance of <see cref="MTexture"/> with the atlas margin set to the provided value.
        /// </summary>
        /// <param name="margin">The margin to use.</param>
        public MTexture WithMargin(int margin) => new MTexture(Data, Identifier, Path, AtlasRectangle, FrameColumns, FrameRows, FrameRate, margin);

        public override string ToString() => $"{Identifier} | {AtlasRectangle}";
    }
}