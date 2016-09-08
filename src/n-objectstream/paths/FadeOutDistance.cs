using System;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
  public class FadeOutDistance : IAnimationPath
  {
    /// Start the fade when this far from the target
    public float Offset { get; set; }

    /// The target object
    public GameObject Target { get; set; }

    /// The offset color
    public Color32 Color { get; set; }

    public FadeOutDistance()
    {
      Color = new Color32(255, 255, 255, 255);
    }

    public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject origin)
    {
      var distance = (Target.transform.position - origin.GameObject.transform.position).magnitude;
      if (distance < Offset)
      {
        var total = 1f - (distance - Offset) / Offset;
        byte alpha = (byte) (255 - (byte) Math.Ceiling(255 * total));
        alpha = transform.Active ? alpha : (byte) 255;
        transform.Color = new Color32(Color.r, Color.g, Color.b, alpha);
      }
    }
  }
}