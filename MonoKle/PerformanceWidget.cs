using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Asset;
using MonoKle.Configuration;
using MonoKle.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoKle
{
    public class PerformanceWidget
    {
        private const int ChartDataLength = 120;
        private const int ChartBottom = 120;
        private const int ChartDelta = 50;
        private const int ChartTop = ChartBottom - ChartDelta;

        private readonly FrameCounter _drawCounter = new FrameCounter();
        private readonly FrameCounter _updateCounter = new FrameCounter();
        private readonly SpriteBatch _spriteBatch;
        private readonly PrimitiveBatch2D _primitiveBatch;
        private readonly FontInstance _font;
        private readonly MTexture _whiteTexture;

        private readonly float[] _chartData = new float[ChartDataLength];
        private int _chartStartIndex = 0;
        private int _chartEndIndex = ChartDataLength - 1;

        public PerformanceWidget(GraphicsDevice graphicsDevice, FontInstance font, MTexture whiteTexture)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _primitiveBatch = new PrimitiveBatch2D(graphicsDevice);
            _font = font;
            _whiteTexture = whiteTexture;
        }

        /// <summary>
        /// Gets or sets whether the widget is enabled.
        /// </summary>
        /// <remarks>
        /// If false, no logic nor drawing happens to preserve performance.
        /// </remarks>
        [CVar("perfWidget_enabled")]
        public bool Enabled { get; set; }

        public void BeginDraw()
        {
            if (Enabled)
            {
                _drawCounter.Begin();
            }
        }

        public void EndDraw()
        {
            if (Enabled && _drawCounter.IsActive)
            {
                var time = _drawCounter.End();
                _chartData[_chartStartIndex] = (float)time.TotalMilliseconds;
            }
        }

        public void BeginUpdate()
        {
            if (Enabled)
            {
                _updateCounter.Begin();
            }
        }

        public void EndUpdate()
        {
            if (Enabled && _updateCounter.IsActive)
            {
                var time = _updateCounter.End();
                _chartData[_chartStartIndex] += (float)time.TotalMilliseconds;
                IncrementChartPointer(ref _chartStartIndex);
                IncrementChartPointer(ref _chartEndIndex);
            }
        }

        private void IncrementChartPointer(ref int index) => index = WrapChartPointer(index + 1);

        private int WrapChartPointer(int index) => index >= ChartDataLength ? 0 : index;

        public void Draw()
        {
            if (!Enabled)
            {
                return;
            }

            var maxValue = _chartData.Max();
            var minValue = _chartData.Min();

            // Draw graph
            _primitiveBatch.Begin();
            int coordinateX = 0;
            // Iterate all points and connect with lines
            for (int i = _chartStartIndex; i != _chartEndIndex; IncrementChartPointer(ref i))
            {
                var first = (_chartData[i] - minValue) / maxValue * ChartDelta;
                var second = (_chartData[WrapChartPointer(i + 1)] - minValue) / maxValue * ChartDelta;

                _primitiveBatch.DrawLine(new MVector2(coordinateX, ChartBottom - first), new MVector2(coordinateX + 1, ChartBottom - second), Color.White);
                coordinateX++;
            }
            // Draw top and bottom limits
            _primitiveBatch.DrawLine(new MVector2(0, ChartTop), new MVector2(coordinateX, ChartTop), Color.White);
            _primitiveBatch.DrawLine(new MVector2(0, ChartBottom), new MVector2(coordinateX, ChartBottom), Color.White);
            _primitiveBatch.End();

            // Draw the text
            var builder = new StringBuilder();
            builder.AppendLine($"Update: {_updateCounter.FrameTime.TotalMilliseconds:0.00ms} ({_updateCounter.FramesPerSecond}/s)");
            builder.AppendLine($"  Draw: {_drawCounter.FrameTime.TotalMilliseconds:0.00ms} ({_drawCounter.FramesPerSecond}/s)");
            builder.AppendLine();
            builder.AppendLine($"Max: {maxValue:0.00ms}\n\n\n\n");
            builder.AppendLine($"Min: {minValue:0.00ms}");

            _spriteBatch.Begin();
            _spriteBatch.Draw(_whiteTexture, new MRectangleInt(0, 0, 220, 155), new Color(0, 0, 0, 100));
            _font.Draw(_spriteBatch, builder.ToString(), MVector2.Zero, Color.White);
            _spriteBatch.End();
        }
    }
}
