using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
  public class LinearFixedPath : IAnimationPath
  {
    public GameObject Origin { get; set; }

    public GameObject Target { get; set; }

    public float Speed { get; set; }

    public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject spawned)
    {
      // Normal linear transforms
      transform.Rotation = Quaternion.Slerp(Origin.transform.rotation, Target.transform.rotation, curve.Value);
      transform.Scale = Vector3.Lerp(Origin.transform.localScale, Target.transform.localScale, curve.Value);

      // Find new position by speed
      var direction = (Target.transform.position - spawned.GameObject.transform.position).normalized;
      var delta = Speed * curve.Delta * direction;
      var output = spawned.GameObject.transform.position + delta;

      // Force position to be on the correct path
      var correctGap = (Target.transform.position - Origin.transform.position);
      var correctDirection = correctGap.normalized;
      var step = (output - Origin.transform.position).magnitude;

      // If the movement moves past the target, halt at target
      if (step > correctGap.magnitude)
      {
        step = correctGap.magnitude;
        transform.Active = false;
      }

      // Save output
      output = Origin.transform.position + step * correctDirection;
      transform.Position = output;
    }
  }
}