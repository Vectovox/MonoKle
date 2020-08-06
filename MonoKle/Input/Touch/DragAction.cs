using System;

namespace MonoKle.Input.Touch
{
    public class DragAction : PressAction, IDragAction
    {
        public MPoint2 Delta
        {
            get
            {
                return IsTriggered
                    ? _delta
                    : throw new InvalidOperationException("Delta not valid because action was not triggered.");
            }
        }
        private MPoint2 _delta;

        public void Set(MPoint2 coordinate, MPoint2 delta)
        {
            Set(coordinate);
            _delta = delta;
        }

        public new void Reset()
        {
            base.Reset();
            _delta = MPoint2.Zero;
        }

        public bool TryGetDelta(out MPoint2 delta)
        {
            delta = _delta;
            return IsTriggered;
        }
    }
}
