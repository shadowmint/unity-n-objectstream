using System;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
    public class FadeOutPath : IAnimationPath
    {
        /// Start the fade at this offset into the animation curve
        public float offset;

        public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject target)
        {
            if (curve.Value > offset)
            {
                var total = 1.0f - offset;
                if (total >= 0)
                {
                    var value = (curve.Value - offset) / total;
                    byte alpha = (byte)(255 - (byte)Math.Ceiling(255 * value));
                    alpha = transform.active ? alpha : (byte)255;
                    transform.color.a = alpha;
                }
            }
        }
    }
}
