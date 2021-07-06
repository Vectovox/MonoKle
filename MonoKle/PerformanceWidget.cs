using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Asset;
using MonoKle.Configuration;
using MonoKle.Graphics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MonoKle
{
    public class PerformanceWidget
    {
        private const int ChartDataLength = 180;
        private const int ChartBottom = 185;
        private const int ChartDelta = 50;
        private const int ChartTop = ChartBottom - ChartDelta;
        
        private const float FrameTime30FPS = 33.33f;
        private const float FrameTime60FPS = 16.66f;

        private readonly FrameCounter _drawCounter = new FrameCounter();
        private readonly FrameCounter _updateCounter = new FrameCounter();
        private readonly SpriteBatch _spriteBatch;
        private readonly PrimitiveBatch2D _primitiveBatch;
        private readonly FontInstance _font;
        private readonly MTexture _whiteTexture;
        private readonly CachedProcessProvider _processProvider;

        private readonly float[] _chartData = new float[ChartDataLength];
        private int _chartStartIndex = 0;
        private int _chartEndIndex = ChartDataLength - 1;

        public PerformanceWidget(GraphicsDevice graphicsDevice, FontInstance font, MTexture whiteTexture)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _primitiveBatch = new PrimitiveBatch2D(graphicsDevice);
            _font = font;
            _whiteTexture = whiteTexture;
            _processProvider = new CachedProcessProvider();
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

            var totalFrameTime = _updateCounter.FrameTime + _drawCounter.FrameTime;
            var theoreticalTotalFPS = (int)(1 / totalFrameTime.TotalSeconds);
            var maxFrameTime = _chartData.Max();
            var minFrameTime = _chartData.Min();
            
            // Draw the text
            var builder = new StringBuilder();
            builder.AppendLine($"= FPS: {_drawCounter.FramesPerSecond}");
            builder.AppendLine();
            builder.AppendLine($"= FRAME TIME");
            builder.AppendLine($"  Total: {totalFrameTime.TotalMilliseconds:0.00ms} ({theoreticalTotalFPS}/s)");
            builder.AppendLine($" Update: {_updateCounter.FrameTime.TotalMilliseconds:0.00ms} ({_updateCounter.TheoreticalFramesPerSecond}/s)");
            builder.AppendLine($"   Draw: {_drawCounter.FrameTime.TotalMilliseconds:0.00ms} ({_drawCounter.TheoreticalFramesPerSecond}/s)");
            builder.AppendLine();
            builder.AppendLine($" MAX: {maxFrameTime:0.00ms}");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine($" MIN: {minFrameTime:0.00ms}");
            builder.AppendLine();
            builder.AppendLine($"= PROCESS");
            builder.AppendLine($" Physical: {_processProvider.Process.WorkingSet64 / 1000000} MB ({_processProvider.Process.PeakWorkingSet64 / 1000000} MB)");
            builder.AppendLine($"    Paged: {_processProvider.Process.PagedMemorySize64 / 1000000} MB ({_processProvider.Process.PeakPagedMemorySize64 / 1000000} MB)");
            builder.AppendLine($"  Virtual: {_processProvider.Process.VirtualMemorySize64 / 1000000} MB ({_processProvider.Process.PeakVirtualMemorySize64 / 1000000} MB)");
            builder.AppendLine($"  Threads: {_processProvider.Process.Threads.Count}");

            var text = builder.ToString();
            var textSize = _font.Measure(text);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_whiteTexture, new MRectangleInt(0, 0, (int)Math.Ceiling(textSize.X), (int)Math.Ceiling(textSize.Y)), new Color(0, 0, 0, 150));
            _font.Draw(_spriteBatch, text, MVector2.Zero, Color.White);
            _spriteBatch.End();

            // Draw graph
            _primitiveBatch.Begin();
            var coordinateX = 0;

            float ScaleGraphValue(float value) => (value - minFrameTime) / maxFrameTime * ChartDelta;
            Color GetGraphColor(float value) => value <= FrameTime60FPS
                ? Lerp(Color.SpringGreen, Color.Orange, value / FrameTime60FPS)
                : Lerp(Color.Orange, Color.Red, value / FrameTime30FPS);

            var deltaXPerStep = textSize.X / ChartDataLength;

            // Iterate all points and connect with lines
            for (int i = _chartStartIndex; i != _chartEndIndex; IncrementChartPointer(ref i))
            {
                var firstValue = _chartData[i];
                var secondValue = _chartData[WrapChartPointer(i + 1)];
                var firstY = ScaleGraphValue(firstValue);
                var secondY = ScaleGraphValue(secondValue);
                var firstX = coordinateX * deltaXPerStep;
                var secondX = (coordinateX + 1) * deltaXPerStep;

                _primitiveBatch.DrawLine(new MVector2(firstX, ChartBottom - firstY), new MVector2(secondX, ChartBottom - secondY), GetGraphColor(firstValue), GetGraphColor(secondValue));
                coordinateX++;
            }

            // Draw top and bottom limits
            _primitiveBatch.DrawLine(new MVector2(0, ChartTop), new MVector2(textSize.X, ChartTop), Color.White);
            _primitiveBatch.DrawLine(new MVector2(0, ChartBottom), new MVector2(textSize.X, ChartBottom), Color.White);
            _primitiveBatch.End();
        }

        private Color Lerp(Color a, Color b, float amount) =>
            new Color(MathHelper.Lerp(a.R, b.R, amount) / 255f, MathHelper.Lerp(a.G, b.G, amount) / 255f, MathHelper.Lerp(a.B, b.B, amount) / 255f, MathHelper.Lerp(a.A, b.A, amount) / 255f);
    }
}
