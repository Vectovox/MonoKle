using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Asset;
using MonoKle.Configuration;
using MonoKle.Graphics;
using System;
using System.Text;

namespace MonoKle
{
    public class PerformanceChartData
    {
        private readonly float[] _chartData;
        private int _chartStartIndex;
        private int _chartEndIndex;

        public int Size => _chartData.Length;

        public PerformanceChartData(int size)
        {
            _chartData = new float[size];
            _chartStartIndex = 0;
            _chartEndIndex = size - 1;
        }

        public void FrameComplete(TimeSpan delta)
        {
            _chartData[_chartStartIndex] = (float)delta.TotalMilliseconds;
            IncrementChartPointer(ref _chartStartIndex);
            IncrementChartPointer(ref _chartEndIndex);
        }

        public (float, float) GetExtremes()
        {
            var maxFrameTime = 0.00001f;
            var minFrameTime = float.MaxValue;

            for (int i = 0; i < _chartData.Length; i++)
            {
                var data = _chartData[i];
                if (data > maxFrameTime)
                {
                    maxFrameTime = data;
                }
                if (data < minFrameTime)
                {
                    minFrameTime = data;
                }
            }

            return (minFrameTime, maxFrameTime);
        }

        public void IterateData(Action<float, float> callback)
        {
            for (int i = _chartStartIndex; i != _chartEndIndex; IncrementChartPointer(ref i))
            {
                var firstValue = _chartData[i];
                var secondValue = _chartData[WrapChartPointer(i + 1)];
                callback(firstValue, secondValue);
            }
        }

        private void IncrementChartPointer(ref int index) => index = WrapChartPointer(index + 1);

        private int WrapChartPointer(int index) => index >= Size ? 0 : index;
    }

    public class PerformanceWidget
    {
        private readonly FrameCounter _drawCounter = new();
        private readonly FrameCounter _updateCounter = new();
        private readonly SpriteBatch _spriteBatch;
        private readonly PrimitiveBatch2D _primitiveBatch;
        private readonly FontInstance _font;
        private readonly MTexture _whiteTexture;
        private readonly CachedProcessProvider _processProvider;
        private readonly PerformanceChartData _drawChartData = new(180);
        private readonly PerformanceChartData _updateChartData = new(180);

        // Fields for calculating text with relatively good performance
        private readonly StringBuilder _textBuilder = new();
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
                _drawChartData.FrameComplete(_drawCounter.End());
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
                _updateChartData.FrameComplete(_updateCounter.End());
            }
        }

        public void Draw()
        {
            if (Level == MeasurementLevel.Disabled)
            {
                return;
            }

            // Calculate max and min frame times
            (float minDrawTime, float maxDrawTime) = _drawChartData.GetExtremes();
            (float minUpdateTime, float maxUpdateTime) = _updateChartData.GetExtremes();

            // Draw the text
            _spriteBatch.Begin();
            var text = GetText(_updateCounter, _drawCounter, maxDrawTime, minDrawTime, maxUpdateTime, minUpdateTime, _processProvider);
            var textSize = _font.Measure(text);
            _spriteBatch.Draw(_whiteTexture, new MRectangleInt(0, 0, (int)Math.Ceiling(textSize.X), (int)Math.Ceiling(textSize.Y)), new Color(0, 0, 0, 150));
            _font.Draw(_spriteBatch, text, MVector2.Zero, Color.White);
            _spriteBatch.End();

            // Draw graph
            if (Level >= MeasurementLevel.FrameTime)
            {
                DrawGraph(_primitiveBatch, _drawChartData, 167, textSize.X, 50, minDrawTime, maxDrawTime);
                DrawGraph(_primitiveBatch, _updateChartData, 245, textSize.X, 50, minUpdateTime, maxUpdateTime);
            }
        }

        private static void DrawGraph(PrimitiveBatch2D batch, PerformanceChartData data, float y, float width, float height, float minFrameTime, float maxFrameTime)
        {
            const float FrameTime30FPS = 33.33f;
            const float FrameTime60FPS = 16.66f;

            var bottom = y + height;

            float ScaleGraphValue(float value) => (value - minFrameTime) / maxFrameTime * height;
            static Color GetGraphColor(float value) => value <= FrameTime60FPS
                ? LerpHelper.Lerp(Color.SpringGreen, Color.Yellow, value / FrameTime60FPS)
                : LerpHelper.Lerp(Color.Yellow, Color.Red, value / FrameTime30FPS);

            var deltaXPerStep = width / data.Size;
            var coordinateX = 0;

            void DrawCallback(float firstValue, float secondValue)
            {
                var firstY = ScaleGraphValue(firstValue);
                var secondY = ScaleGraphValue(secondValue);
                var firstX = coordinateX * deltaXPerStep;
                var secondX = (coordinateX + 1) * deltaXPerStep;

                batch.DrawLine(new MVector2(firstX, bottom - firstY), new MVector2(secondX, bottom - secondY), GetGraphColor(firstValue), GetGraphColor(secondValue));
                coordinateX++;
            }

            batch.Begin();
            // Iterate all points and connect with lines
            data.IterateData(DrawCallback);
            // Draw top and bottom limits
            batch.DrawLine(new MVector2(0, y), new MVector2(width, y), Color.White);
            batch.DrawLine(new MVector2(0, bottom), new MVector2(width, bottom), Color.White);
            batch.End();
        }

        public string GetText(FrameCounter _updateCounter, FrameCounter _drawCounter, float maxDrawTime, float minDrawTime, float maxUpdateTime, float minUpdateTime, CachedProcessProvider processProvider)
        {
            var totalFrameTime = _updateCounter.FrameTime + _drawCounter.FrameTime;

            // Check if we should update string
            if (_cachedTotalFrameTime != totalFrameTime)
            {
                _cachedTotalFrameTime = totalFrameTime;
                var theoreticalTotalFPS = (int)(1 / totalFrameTime.TotalSeconds);

                _textBuilder.Clear();
                _textBuilder.AppendLine($"= FRAMES");
                _textBuilder.AppendLine($"   Draw: {_drawCounter.FramesPerSecond}");
                _textBuilder.AppendLine($" Update: {_updateCounter.FramesPerSecond}");

                if (Level >= MeasurementLevel.FrameTime)
                {
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine($"= FRAME TIME");
                    _textBuilder.AppendLine($"  Total: {totalFrameTime.TotalMilliseconds:0.00ms} ({theoreticalTotalFPS}/s)");
                    _textBuilder.AppendLine($"   Draw: {_drawCounter.FrameTime.TotalMilliseconds:0.00ms} ({_drawCounter.TheoreticalFramesPerSecond}/s)");
                    _textBuilder.AppendLine($" Update: {_updateCounter.FrameTime.TotalMilliseconds:0.00ms} ({_updateCounter.TheoreticalFramesPerSecond}/s)");
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine($" DRAW {minDrawTime:0.00ms} / {maxDrawTime:0.00ms}");
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine($" UPDATE: {minUpdateTime:0.00ms} / {maxUpdateTime:0.00ms}");
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
                    _textBuilder.AppendLine();
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
