using UnityEngine;
using N.Package.Animation;
using N.Package.Core;

namespace N.Package.ObjectStream
{
  /// The transform to apply to an object at a specific point in it's
  /// path. Notice that the alpha and color parameters will only apply
  /// if the material supports them.
  public class PathTransform
  {
    /// Object color tint
    public Color32 Color;

    /// The position
    public Vector3 Position;

    /// The rotation
    public Quaternion Rotation;

    /// The local scale
    public Vector3 Scale;

    /// Set this if for some reason the object has expired, eg. moved
    /// outside of its bounds, etc. A specific path may pick this up
    /// to (for example) hide or make an object transparent.
    public bool Active;

    /// Create an empty transform
    public PathTransform()
    {
      Unity();
    }

    /// Create from game object
    public PathTransform(GameObject target)
    {
      From(target.transform);
    }

    /// Create from transform
    public PathTransform(Transform transform)
    {
      From(transform);
    }

    /// Reset values
    public void Unity()
    {
      Color = new Color32(255, 255, 255, 255);
      Position = default(Vector3);
      Rotation = default(Quaternion);
      Scale = new Vector3(1f, 1f, 1f);
      Active = true;
    }

    /// Load from target
    public void From(Transform transform)
    {
      Position = transform.position;
      Scale = transform.localScale;
      Rotation = transform.rotation;
      Color = default(Color32);
      Active = true;
    }

    /// Load from target
    public void From(NTransform transform)
    {
      Position = transform.Position;
      Scale = transform.Scale;
      Rotation = transform.Rotation;
      Color = default(Color32);
      Active = true;
    }
  }
}