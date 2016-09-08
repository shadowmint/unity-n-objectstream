using System.Collections.Generic;
using UnityEngine;

namespace N.Package.ObjectStream
{
  /// Custom animation target for better performance
  public class SingleSpawned : ISpawnedObjects
  {
    /// The spawned object
    public SpawnedObject Target;

    /// Create a new instance
    public SingleSpawned(SpawnedObject target)
    {
      Target = target;
      Target.GameObject.transform.name = "SpawnedObject";
    }

    /// Remove a game object from this target
    public void Remove(GameObject target)
    {
      if (Target.GameObject == target)
      {
        Target = null;
      }
    }

    /// Yield game objects and renderers
    public IEnumerable<GameObject> GameObjects()
    {
      if (Target != null)
      {
        yield return Target.GameObject;
      }
    }

    /// Yield game objects and renderers
    public IEnumerable<SpawnedObject> MetaObjects()
    {
      if (Target != null)
      {
        yield return Target;
      }
    }
  }
}