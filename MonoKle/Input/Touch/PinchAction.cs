using System;

namespace MonoKle.Input.Touch
{
    public class PinchAction : IPinchAction
    {
        public MPoint2 Origin => IsTriggered
            ? _origin
            : throw new InvalidOperationException($"{nameof(Origin)} not valid because action was not triggered.");
        private MPoint2 _origin;

        public float PinchFactor => IsTriggered
            ? _pinchFactor
            : throw new InvalidOperationException($"{nameof(PinchFactor)} not valid because action was not triggered.");
        private float _pinchFactor;

        public bool IsTriggered { get; private set; }

        public void Set(MPoint2 origin, float pinchFactor)
        {
            _origin = origin;
            _pinchFactor = pinchFactor;
            IsTriggered = true;
        }

        public void Reset()
        {
            _origin = MPoint2.Zero;
            _pinchFactor = 0f;
            IsTriggered = false;
        }

        public bool TryGetValues(out MPoint2 origin, out float pinchFactor)
        {
            origin = _origin;
            pinchFactor = _pinchFactor;
            return IsTriggered;
        }
    }
}
