using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Asset;
using MonoKle.Configuration;
using MonoKle.Graphics;
using System;
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

        // Fields for calculating text with relatively good performance
        private readonly StringBuilder _textBuilder = new StringBuilder();
        private string _cachedText = string.Empty;
        private TimeSpan _cachedTotalFrameTime;

        public PerformanceWidget(GraphicsDevice graphicsDevice, FontInstance font, MTexture whiteTexture)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _primitiveBatch = new PrimitiveBatch2D(graphicsDevice);
            _font = font;
            _whiteTexture = whiteTexture;
            _processProvider = new CachedProcessProvider();
        }

        /// <summary>
        /// Gets or sets the information the widget should display.
        /// </summary>
        /// <remarks>
        /// If set to <see cref="MeasurementLevel.Disabled"/>, no logic nor drawing happens to preserve performance.
        /// </remarks>
        [CVar("perfWidget_level")]
        public MeasurementLevel Level { get; set; }

        public void BeginDraw()
        {
            if (Level != MeasurementLevel.Disabled)
            {
                _drawCounter.Begin();
            }
        }

        public void EndDraw()
        {
            if (Level != MeasurementLevel.Disabled && _drawCounter.IsActive)
            {
                var time = _drawCounter.End();
                _chartData[_chartStartIndex] = (float)time.TotalMilliseconds;
            }
        }

        public void BeginUpdate()
        {
            if (Level != MeasurementLevel.Disabled)
            {
                _updateCounter.Begin();
            }
        }

        public void EndUpdate()
        {
            if (Level != MeasurementLevel.Disabled && _updateCounter.IsActive)
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
            if (Level == MeasurementLevel.Disabled)
            {
                return;
            }

            // Calculate max and min frame times
            var maxFrameTime = 0.00001f;
            var minFrameTime = float.MaxValue;
            foreach (var data in _chartData)
            {
                if (data > maxFrameTime)
                {
                    maxFrameTime = data;
                }
                if (data < minFrameTime)
                {
                    minFrameTime = data;
                }
            }

            // Draw the text
            _spriteBatch.Begin();
            var text = GetText(_updateCounter, _drawCounter, maxFrameTime, minFrameTime, _processProvider);
            var textSize = _font.Measure(text);
            _spriteBatch.Draw(_whiteTexture, new MRectangleInt(0, 0, (int)Math.Ceiling(textSize.X), (int)Math.Ceiling(textSize.Y)), new Color(0, 0, 0, 150));
            _font.Draw(_spriteBatch, text, MVector2.Zero, Color.White);
            _spriteBatch.End();

            // Draw graph
            if (Level >= MeasurementLevel.FrameTime)
            {
                _primitiveBatch.Begin();
                var coordinateX = 0;

                float ScaleGraphValue(float value) => (value - minFrameTime) / maxFrameTime * ChartDelta;
                Color GetGraphColor(float value) => value <= FrameTime60FPS
                    ? LerpHelper.Lerp(Color.SpringGreen, Color.Yellow, value / FrameTime60FPS)
                    : LerpHelper.Lerp(Color.Yellow, Color.Red, value / FrameTime30FPS);

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
        }

        public string GetText(FrameCounter _updateCounter, FrameCounter _drawCounter, float maxFrameTime, float minFrameTime, CachedProcessProvider processProvider)
        {
            var totalFrameTime = _updateCounter.FrameTime + _drawCounter.FrameTime;

            // Check if we should update string
            if (_cachedTotalFrameTime != totalFrameTime)
            {
                _cachedTotalFrameTime = totalFrameTime;
                var theoreticalTotalFPS = (int)(1 / totalFrameTime.TotalSeconds);

                _textBuilder.Clear();
                _textBuilder.AppendLine($"= FPS: {_drawCounter.FramesPerSecond}");
                _textBuilder.AppendLine();

                if (Level >= MeasurementLevel.FrameTime)
                {
                    _textBuilder.AppendLine($"= FRAME TIME");
                    _textBuilder.AppendLine($"  Total: {totalFrameTime.TotalMilliseconds:0.00ms} ({theoreticalTotalFPS}/s)");
                    _textBuilder.AppendLine($" Update: {_updateCounter.FrameTime.TotalMilliseconds:0.00ms} ({_updateCounter.TheoreticalFramesPerSecond}/s)");
                    _textBuilder.AppendLine($"   Draw: {_drawCounter.FrameTime.TotalMilliseconds:0.00ms} ({_drawCounter.TheoreticalFramesPerSecond}/s)");
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine($" MAX: {maxFrameTime:0.00ms}");
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine($" MIN: {minFrameTime:0.00ms}");
                }

                if (Level >= MeasurementLevel.Process)
                {
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine($"= PROCESS");
                    _textBuilder.AppendLine($" Physical: {processProvider.Process.WorkingSet64 / 1000000} MB ({processProvider.Process.PeakWorkingSet64 / 1000000} MB)");
                    _textBuilder.AppendLine($"    Paged: {processProvider.Process.PagedMemorySize64 / 1000000} MB ({processProvider.Process.PeakPagedMemorySize64 / 1000000} MB)");
                    _textBuilder.AppendLine($"  Virtual: {processProvider.Process.VirtualMemorySize64 / 1000000} MB ({processProvider.Process.PeakVirtualMemorySize64 / 1000000} MB)");
                    _textBuilder.AppendLine($"  Threads: {processProvider.Process.Threads.Count}");
                }

                _cachedText = _textBuilder.ToString();
            }

            return _cachedText;
        }

        public enum MeasurementLevel
        {
            Disabled = 0,
            FPS = 1,
            FrameTime = 2,
            Process = 3,
        }
    }
}
