using System.Collections.Generic;
using UnityEngine;

namespace N.Package.ObjectStream
{
    /// Custom animation target for better performance
    public class SingleSpawned : ISpawnedObjects
    {
        /// The spawned object
        public SpawnedObject target;

        /// Create a new instance
        public SingleSpawned(SpawnedObject target)
        {
            this.target = target;
            this.target.gameObject.transform.name = "SpawnedObject";
        }

        /// Remove a game object from this target
        public void Remove(GameObject target)
        {
            if (this.target.gameObject == target)
            {
                this.target = null;
            }
        }

        /// Yield game objects and renderers
        public IEnumerable<GameObject> GameObjects()
        {
            if (target != null)
            {
                yield return target.gameObject;
            }
            yield break;
        }

        /// Yield game objects and renderers
        public IEnumerable<SpawnedObject> MetaObjects()
        {
            if (target != null)
            {
                yield return target;
            }
            yield break;
        }
    }
}
