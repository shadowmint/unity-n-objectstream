using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
    public class ArcFixedPath : IAnimationPath
    {
        public GameObject origin;

        public GameObject target;

        public float speed;

        public float height;

        public Vector3 up;

        private const float CUTOFF_DISTANCE = 0.1f;

        public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject spawned)
        {
            // Normal linear transforms
            transform.scale = Vector3.Lerp(origin.transform.localScale, target.transform.localScale, curve.Value);

            // Find new position by speed
            var direction = (target.transform.position - spawned.gameObject.transform.position).normalized;
            var delta = speed * curve.Delta * direction;
            var output = spawned.gameObject.transform.position + delta;
            if (direction.magnitude == 0f)
            { transform.active = false; return; }

            // Force position to be on the correct path
            var correctDirection = (origin.transform.position - target.transform.position).normalized;
            var offset = output - spawned.origin.position;
            var pathLength = (correctDirection * Vector3.Dot(offset, correctDirection) / correctDirection.magnitude);
            var projected = origin.transform.position + pathLength;

            // Distance from here to origin
            var left = (projected - target.transform.position).magnitude;
            var total = (spawned.origin.position - target.transform.position).magnitude;
            var increment = 1f - left / total;

            // Apply arc
            // NB. f(x)  -> (x, sin(x)) ie. f'(x) -> (1, cos(x))
            var currentHeight = Mathf.Sin(increment * 180f * Mathf.Deg2Rad) * this.height;
            var right = Vector3.Cross(up, direction);
            var tangentDirection = Mathf.Cos(increment * 180f * Mathf.Deg2Rad) * up - correctDirection;
            var normal = Vector3.Cross(right, -tangentDirection);
            transform.rotation = Quaternion.LookRotation(tangentDirection, normal);
            transform.position = projected + up * currentHeight;
            spawned.origin.position = origin.transform.position;

            // Halt?
            if ((transform.position - target.transform.position).magnitude < ArcFixedPath.CUTOFF_DISTANCE)
            { transform.active = false; }
        }
    }
}
