using System;

namespace MonoKle.Input.Touch
{
    public class TouchAction : ITouchAction
    {
        public bool IsTriggered { get; private set; }
        public MPoint2 Coordinate
        {
            get
            {
                return IsTriggered
                    ? _coordinate
                    : throw new InvalidOperationException("Coordinate not valid because action was not triggered.");
            }
        }
        private MPoint2 _coordinate;

        public void Set(MPoint2 location)
        {
            _coordinate = location;
            IsTriggered = true;
        }

        public void Reset()
        {
            _coordinate = MPoint2.Zero;
            IsTriggered = false;
        }

        public bool TryGetCoordinate(out MPoint2 coordinate)
        {
            coordinate = _coordinate;
            return IsTriggered;
        }
    }
}
