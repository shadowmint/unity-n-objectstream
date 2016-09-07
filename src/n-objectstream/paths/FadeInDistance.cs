using System;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
    public class FadeInDistance : IAnimationPath
    {
        /// Fade in over this distance
        public float distance;

        public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject target)
        {
            var delta = (target.GameObject.transform.position - target.Origin.Position).magnitude;
            if (distance > 0)
            {
                if (delta <= distance)
                {
                    var total = delta / distance;
                    byte alpha = (byte)Math.Ceiling(255 * total);
                    transform.color.a = alpha;
                }
            }
        }
    }
}
