using System;

namespace MonoKle.Input.Touch
{
    public class TouchInput : ITouchInput
    {
        public IInputPosition Position => _position;
        private InputPosition _position = new InputPosition();

        public IPressable Press => _press;
        private Button _press = new Button();

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