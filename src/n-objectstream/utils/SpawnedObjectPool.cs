using System;
using System.Collections.Generic;
using UnityEngine;
using N;
using N.Package.Core;

namespace N.Package.ObjectStream
{
  /// The information about a spawned object
  public class SpawnedObject
  {
    public GameObject GameObject;
    public NTransform Origin;
    public Renderer Renderer;
  }

  /// A pool of objects to use repeatedly in a scene
  public class SpawnedObjectPool
  {
    /// The prefab instance to use
    private readonly GameObject _factory;

    /// Set of instances
    private readonly List<SpawnedObject> _instances = new List<SpawnedObject>();

    /// Limit
    private Option<int> limit;

    /// Create an instance and provide a prefab factory to it to use
    /// @param factory A wrapped gameobject, use Scene.Prefab()
    /// @param limit The limit, if any, to the count of objects to spawn.
    public SpawnedObjectPool(Option<GameObject> factory, int limit = -1)
    {
      if (factory)
      {
        this._factory = factory.Unwrap();
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
      if (stored)
      {
        return stored;
      }
      return NewInstance(origin);
    }

    /// Create a new instance
    private Option<SpawnedObject> NewInstance(NTransform origin)
    {
      if (limit)
      {
        if (_instances.Count >= limit.Unwrap())
        {
          return Option.None<SpawnedObject>();
        }
      }
      var rtn = Scene.Spawn(_factory);
      if (rtn)
      {
        var gp = rtn.Unwrap();
        var instance = new SpawnedObject
        {
          GameObject = gp,
          Origin = origin,
          Renderer = gp.GetComponent<Renderer>()
        };
        ApplyTransform(instance);
        _instances.Add(instance);
        return Option.Some(instance);
      }
      return Option.None<SpawnedObject>();
    }

    /// Return the next free instance
    private Option<SpawnedObject> NextFree(NTransform origin)
    {
      SpawnedObject match = null;
      foreach (var instance in _instances)
      {
        if (!instance.GameObject.activeSelf)
        {
          match = instance;
          break;
        }
      }
      if (match != null)
      {
        match.GameObject.SetActive(true);
        match.Origin = origin;
        ApplyTransform(match);
        return Option.Some(match);
      }
      return Option.None<SpawnedObject>();
    }

    /// Apply a transform
    private void ApplyTransform(SpawnedObject target)
    {
      target.GameObject.transform.position = target.Origin.Position;
      target.GameObject.transform.rotation = target.Origin.Rotation;
      target.GameObject.transform.localScale = target.Origin.Scale;
    }

    /// Drop all instances
    public void Clear()
    {
      foreach (var instance in _instances)
      {
        GameObject.Destroy(instance.GameObject);
      }
      _instances.Clear();
    }

    /// Return a count of active objects
    public int Active
    {
      get
      {
        var count = 0;
        foreach (var instance in _instances)
        {
          if (instance.GameObject.activeSelf)
          {
            count += 1;
          }
        }
        return count;
      }
    }
  }
}