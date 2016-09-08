using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
  public class LinearPath : IAnimationPath
  {
    public GameObject Origin { get; set; }

    public GameObject Target { get; set; }

    public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject spawned)
    {
      transform.Position = Vector3.Lerp(Origin.transform.position, Target.transform.position, curve.Value);
      transform.Rotation = Quaternion.Slerp(Origin.transform.rotation, Target.transform.rotation, curve.Value);
      transform.Scale = Vector3.Lerp(Origin.transform.localScale, Target.transform.localScale, curve.Value);
    }
  }
}