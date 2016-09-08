using System.Collections.Generic;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream
{
  /// Custom animation target for better performance
  public interface ISpawnedObjects : IAnimationTarget
  {
    /// Yield game objects and renderers
    IEnumerable<SpawnedObject> MetaObjects();

    /// Remove a game object from this target
    void Remove(GameObject target);
  }
}