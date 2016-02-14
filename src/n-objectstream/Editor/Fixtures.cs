using N.Package.Animation;

namespace N.Package.ObjectStream.Tests
{
    public class FakeCurve : IAnimationCurve
    {
        public float duration = 0f;

        public float elapsed = 0f;

        /// The last delta
        public float Delta { get; set; }

        /// The total time elasped in this animation curve
        public float Elapsed
        {
            get
            {
                return elapsed;
            }
            set
            {
                elapsed = value;
                if (elapsed > duration)
                {
                    elapsed = duration;
                }
            }
        }

        /// The total length of time remaining on this animation curve.
        /// If this value is -ve then the animation curve should never end.
        public float Remaining
        { get { return duration - Elapsed; } }

        /// The current value of the curve, given Elapsed and Remaining.
        public float Value
        { get { return elapsed / duration; } }

        /// Finished yet?
        public bool Complete
        { get { return Value >= 1.0; } }

        public void Halt()
        { elapsed = duration; }
    }
}
