using System;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
  public class FadeOutPath : IAnimationPath
  {
    /// Start the fade at this offset into the animation curve
    public float Offset;

    public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject target)
    {
      if (curve.Value > Offset)
      {
        var total = 1.0f - Offset;
        if (total >= 0)
        {
          var value = (curve.Value - Offset) / total;
          byte alpha = (byte) (255 - (byte) Math.Ceiling(255 * value));
          alpha = transform.Active ? alpha : (byte) 255;
          transform.Color.a = alpha;
        }
      }
    }
  }
}