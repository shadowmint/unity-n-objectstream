using System;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
  public class ArcFixedPath : IAnimationPath
  {
    public GameObject Origin;

    public GameObject Target;

    public float Speed;

    public float Height;

    public Vector3 Up;

    private const float CutoffDistance = 0.1f;
    private const float DirectionTolerance = 0.001f;

    public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject spawned)
    {
      // Normal linear transforms
      transform.scale = Vector3.Lerp(Origin.transform.localScale, Target.transform.localScale, curve.Value);

      // Find new position by speed
      var direction = (Target.transform.position - spawned.GameObject.transform.position).normalized;
      var delta = Speed*curve.Delta*direction;
      var output = spawned.GameObject.transform.position + delta;
      if (Math.Abs(direction.magnitude) < DirectionTolerance)
      {
        transform.active = false;
        return;
      }

      // Force position to be on the correct path
      var correctDirection = (Origin.transform.position - Target.transform.position).normalized;
      var offset = output - spawned.Origin.Position;
      var pathLength = (correctDirection*Vector3.Dot(offset, correctDirection)/correctDirection.magnitude);
      var projected = Origin.transform.position + pathLength;

      // Distance from here to origin
      var left = (projected - Target.transform.position).magnitude;
      var total = (spawned.Origin.Position - Target.transform.position).magnitude;
      var increment = 1f - left/total;

      // Apply arc
      // NB. f(x)  -> (x, sin(x)) ie. f'(x) -> (1, cos(x))
      var currentHeight = Mathf.Sin(increment*180f*Mathf.Deg2Rad)*this.Height;
      var right = Vector3.Cross(Up, direction);
      var tangentDirection = Mathf.Cos(increment*180f*Mathf.Deg2Rad)*Up - correctDirection;
      var normal = Vector3.Cross(right, -tangentDirection);
      transform.rotation = Quaternion.LookRotation(tangentDirection, normal);
      transform.position = projected + Up*currentHeight;
      spawned.Origin.Position = Origin.transform.position;

      // Halt?
      if ((transform.position - Target.transform.position).magnitude < ArcFixedPath.CutoffDistance)
      {
        transform.active = false;
      }
    }
  }
}