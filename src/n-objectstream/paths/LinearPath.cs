using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream.Paths
{
    public class LinearPath : IAnimationPath
    {
        public GameObject origin;

        public GameObject target;

        public void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject spawned)
        {
            transform.position = Vector3.Lerp(origin.transform.position, target.transform.position, curve.Value);
            transform.rotation = Quaternion.Slerp(origin.transform.rotation, target.transform.rotation, curve.Value);
            transform.scale = Vector3.Lerp(origin.transform.localScale, target.transform.localScale, curve.Value);
        }
    }
}
