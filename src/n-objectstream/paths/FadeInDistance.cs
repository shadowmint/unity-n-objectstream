using System;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
  public class FadeInDistance : IAnimationPath
  {
    /// Fade in over this distance
    public float Distance { get; set; }

    public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject target)
    {
      var delta = (target.GameObject.transform.position - target.Origin.Position).magnitude;
      if (Distance > 0)
      {
        if (delta <= Distance)
        {
          var total = delta / Distance;
          byte alpha = (byte) Math.Ceiling(255 * total);
          transform.Color.a = alpha;
        }
      }
    }
  }
}