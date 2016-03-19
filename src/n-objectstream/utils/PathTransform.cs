using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream
{
    /// The transform to apply to an object at a specific point in it's
    /// path. Notice that the alpha and color parameters will only apply
    /// if the material supports them.
    public class PathTransform
    {
        /// Object color tint
        public Color32 color;

        /// The position
        public Vector3 position;

        /// The rotation
        public Quaternion rotation;

        /// The local scale
        public Vector3 scale;

        /// Set this if for some reason the object has expired, eg. moved
        /// outside of its bounds, etc. A specific path may pick this up
        /// to (for example) hide or make an object transparent.
        public bool active;

        /// Create an empty transform
        public PathTransform()
        { Unity(); }

        /// Create from game object
        public PathTransform(GameObject target)
        { From(target.transform); }

        /// Create from transform
        public PathTransform(Transform transform)
        { From(transform); }

        /// Reset values
        public void Unity() {
            color = new Color32(255, 255, 255, 255);
            position = default(Vector3);
            rotation = default(Quaternion);
            scale = new Vector3(1f, 1f, 1f);
            active = true;
        }

        /// Load from target
        public void From(Transform transform)
        {
            position = transform.position;
            scale = transform.localScale;
            rotation = transform.rotation;
            color = default(Color32);
            active = true;
        }

        /// Load from target
        public void From(NTransform transform)
        {
            position = transform.position;
            scale = transform.scale;
            rotation = transform.rotation;
            color = default(Color32);
            active = true;
        }
    }
}
