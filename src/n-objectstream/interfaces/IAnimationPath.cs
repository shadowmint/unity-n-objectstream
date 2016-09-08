using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream
{
  /// This is the base interface for a path to project objects along.
  /// A path generates a PathTransform object for a specific depth into
  /// the animation's IAnimationCurve.
  ///
  /// Notice that an IAnimationPath is a destructive modifier to a PathTransform
  /// object; multiple instances can be applied to a single transform, eg. for
  /// transparency, orientation, position, etc.
  ///
  /// The output of a single IAnimationPath may not directly map to the output
  /// applied to the object stream.
  public interface IAnimationPath
  {
    /// Modify the given PathTransform as required for this pass.
    /// @para curve The animation curve
    /// @param transform The transform to update
    /// @param target The original target, eg. for 'distance from origin'
    void Update(IAnimationCurve curve, PathTransform transform, SpawnedObject target);
  }
}