using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
    public class LinearFixedPath : IAnimationPath
    {
        public GameObject origin;

        public GameObject target;

        public float speed;

        public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject spawned)
        {
            // Normal linear transforms
            transform.rotation = Quaternion.Slerp(origin.transform.rotation, target.transform.rotation, curve.Value);
            transform.scale = Vector3.Lerp(origin.transform.localScale, target.transform.localScale, curve.Value);

            // Find new position by speed
            var direction = (target.transform.position - spawned.GameObject.transform.position).normalized;
            var delta = speed * curve.Delta * direction;
            var output = spawned.GameObject.transform.position + delta;

            // Force position to be on the correct path
            var correctGap = (target.transform.position - origin.transform.position);
            var correctDirection = correctGap.normalized;
            var step = (output - origin.transform.position).magnitude;

            // If the movement moves past the target, halt at target
            if (step > correctGap.magnitude)
            {
                step = correctGap.magnitude;
                transform.active = false;
            }

            // Save output
            output = origin.transform.position + step * correctDirection;
            transform.position = output;
        }
    }
}
