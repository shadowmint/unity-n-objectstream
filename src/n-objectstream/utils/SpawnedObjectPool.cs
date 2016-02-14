using System;
using System.Collections.Generic;
using UnityEngine;
using N;

namespace N.Package.ObjectStream
{
    /// The information about a spawned object
    public class SpawnedObject
    {
        public GameObject gameObject;
        public NTransform origin;
        public Renderer renderer;
    }

    /// A pool of objects to use repeatedly in a scene
    public class SpawnedObjectPool
    {

        /// The prefab instance to use
        private GameObject factory;

        /// Set of instances
        private List<SpawnedObject> instances = new List<SpawnedObject>();

        /// Limit
        private Option<int> limit;

        /// Create an instance and provide a prefab factory to it to use
        /// @param factory A wrapped gameobject, use Scene.Prefab()
        /// @param limit The limit, if any, to the count of objects to spawn.
        public SpawnedObjectPool(Option<GameObject> factory, int limit = -1)
        {
            if (factory)
            {
                this.factory = factory.Unwrap();
                this.limit = limit >= 0 ? Option.Some(limit) : Option.None<int>();
                return;
            }
            throw new Exception("Unable to create object pool from null prefab");
        }

        /// Get an instance
        /// To release an instance, simply set active to false.
        public Option<SpawnedObject> Instance(NTransform origin)
        {
            var stored = NextFree(origin);
            if (stored) { return stored; }
            return NewInstance(origin);
        }

        /// Create a new instance
        private Option<SpawnedObject> NewInstance(NTransform origin)
        {
            if (limit)
            {
                if (instances.Count >= limit.Unwrap())
                {
                    return Option.None<SpawnedObject>();
                }
            }
            var rtn = Scene.Spawn(factory);
            if (rtn)
            {
                var gp = rtn.Unwrap();
                var instance = new SpawnedObject
                {
                    gameObject = gp,
                    origin = origin,
                    renderer = gp.GetComponent<Renderer>()
                };
                ApplyTransform(instance);
                instances.Add(instance);
                return Option.Some(instance);
            }
            return Option.None<SpawnedObject>();
        }

        /// Return the next free instance
        private Option<SpawnedObject> NextFree(NTransform origin)
        {
            SpawnedObject match = null;
            foreach (var instance in instances)
            {
                if (!instance.gameObject.activeSelf)
                {
                    match = instance;
                    break;
                }
            }
            if (match != null)
            {
                match.gameObject.SetActive(true);
                match.origin = origin;
                ApplyTransform(match);
                return Option.Some(match);
            }
            return Option.None<SpawnedObject>();
        }

        /// Apply a transform
        private void ApplyTransform(SpawnedObject target)
        {
            target.gameObject.transform.position = target.origin.position;
            target.gameObject.transform.rotation = target.origin.rotation;
            target.gameObject.transform.localScale = target.origin.scale;
        }

        /// Drop all instances
        public void Clear()
        {
            foreach (var instance in instances)
            {
                GameObject.Destroy(instance.gameObject);
            }
            instances.Clear();
        }

        /// Return a count of active objects
        public int Active
        {
            get
            {
                var count = 0;
                foreach (var instance in instances)
                {
                    if (instance.gameObject.activeSelf)
                    {
                        count += 1;
                    }
                }
                return count;
            }
        }
    }
}
