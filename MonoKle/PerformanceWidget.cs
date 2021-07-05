using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoKle.Asset;
using MonoKle.Configuration;
using MonoKle.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoKle
{
    public class PerformanceWidget
    {
        private readonly FrameCounter _drawCounter = new FrameCounter();
        private readonly FrameCounter _updateCounter = new FrameCounter();
        private readonly SpriteBatch _spriteBatch;
        private readonly PrimitiveBatch2D _primitiveBatch;
        private readonly FontInstance _font;

        public PerformanceWidget(GraphicsDevice graphicsDevice, FontInstance font)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _primitiveBatch = new PrimitiveBatch2D(graphicsDevice);
            _font = font;
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
                _drawCounter.End();
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
                _updateCounter.End();
            }
        }

        public void Draw()
        {
            if (!Enabled)
            {
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Update: {_updateCounter.TimePerUpdate.TotalMilliseconds:0.00ms}");
            builder.AppendLine($"  Draw: {_drawCounter.TimePerUpdate.TotalMilliseconds:0.00ms}");

            _spriteBatch.Begin();
            _font.Draw(_spriteBatch, builder.ToString(), MVector2.Zero, Color.White);
            _spriteBatch.End();
        }
    }
}
