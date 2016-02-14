using System;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
    public class FadeOutDistance : IAnimationPath
    {
        /// Start the fade when this far from the target
        public float offset;

        /// The target object
        public GameObject target;

        public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject origin)
        {
            var distance = (target.transform.position - origin.gameObject.transform.position).magnitude;
            if (distance < offset)
            {
                var total = 1f - (distance - offset) / offset;
                byte alpha = (byte)(255 - (byte)Math.Ceiling(255 * total));
                alpha = transform.active ? alpha : (byte)255;
                transform.color.a = alpha;
            }
        }
    }
}
