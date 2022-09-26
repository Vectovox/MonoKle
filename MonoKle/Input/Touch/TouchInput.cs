using System;

namespace MonoKle.Input.Touch
{
    public class TouchInput : ITouchInput
    {
        public IInputPosition Position => _position;
        private readonly InputPosition _position = new();

        public IPressable Press => _press;
        private readonly Button _press = new();

        public void Reset(TimeSpan timeDelta)
        {
            _press.Update(false, timeDelta);
        }

        public void Update(TimeSpan timeDelta, MPoint2 point)
        {
            _press.Update(true, timeDelta);
            _position.Update(point);
        }
    }
}