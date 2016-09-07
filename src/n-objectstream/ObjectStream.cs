using UnityEngine;
using N.Package.Animation;
using N.Package.Core;
using N.Package.Events;

namespace N.Package.ObjectStream
{
  /// All the data to spawn an object
  public struct SpawnData<TStream>
  {
    /// The animation manager to use
    public AnimationManagerBase<TStream> manager;

    /// The animation stream to use
    public TStream stream;

    /// The path to use for the animation
    public IPathGroup path;

    /// The curve to use for the animation
    public IAnimationCurve curve;

    /// The spawn point
    public NTransform origin;
  }

  /// Return data for spawning an animation
  public delegate SpawnData<TStream> SpawnCallback<TStream>();

  /// Custom animation target for better performance
  public class ObjectStream<TStream> : IHaltable
  {
    /// The objects pool to get objects instances from
    private readonly SpawnedObjectPool _pool;

    /// Time since last spawn
    private float _lastSpawn;

    /// How often to spawn objects
    public float SpawnObjectEvery;

    /// Total elapsed time
    public float Elapsed;

    /// Currently suspeneded
    private bool _halted;

    /// The factory callback
    private readonly SpawnCallback<TStream> _factory;

    /// Create a new instance
    /// @param template The template GameObject to spawn instances of
    /// @param spawnObjectEvery Spawn a new instance this often
    /// @param maxPoolSize Allow at most this many objects to exist
    /// @param callback Callback to provide animation curve, etc.
    public ObjectStream(Option<GameObject> template, float spawnObjectEvery, int maxPoolSize, SpawnCallback<TStream> factory)
    {
      this.SpawnObjectEvery = spawnObjectEvery;
      this._factory = factory;
      _pool = new SpawnedObjectPool(template, maxPoolSize);
      Elapsed = spawnObjectEvery;
    }

    /// Create a new instance
    /// @param template The template GameObject to spawn instances of
    /// @param spawnObjectEvery Spawn a new instance this often
    /// @param maxPoolSize Allow at most this many objects to exist
    /// @param callback Callback to provide animation curve, etc.
    public ObjectStream(GameObject template, float spawnObjectEvery, int maxPoolSize, SpawnCallback<TStream> factory)
    {
      this.SpawnObjectEvery = spawnObjectEvery;
      this._factory = factory;
      _pool = new SpawnedObjectPool(Option.Some(template), maxPoolSize);
      Elapsed = spawnObjectEvery;
    }

    /// Spawn a new object from the object pool
    /// Add new objects, delete old objects, etc.
    public void Update(float delta)
    {
      if (!_halted)
      {
        Elapsed += delta;
        if ((Elapsed - _lastSpawn) > SpawnObjectEvery)
        {
          var req = _factory();
          if (req.manager == null) return;

          _pool.Instance(req.origin).Then((gp) =>
          {
            // Start animation
            var animation = new FollowPathAnimation(this);
            animation.pathGroup = req.path;
            var target = new SingleSpawned(gp);

            req.manager.Add(req.stream, animation, req.curve, target);
            _lastSpawn = Elapsed;
            var context = new EventContext();
            req.manager.Events.AddEventHandler<AnimationCompleteEvent>((ep) =>
            {
              if (ep.animation != animation) return;
              gp.GameObject.SetActive(false); // return object to pool
              context.Dispose();
            }, context);
          });
        }
      }
    }

    /// Halt the object stream, end animations
    /// NB. This stops the stream and resets the object start, it doesn't pause.
    public void Halt()
    {
      _halted = true;
    }

    /// Resume the object stream from scratch.
    public void Resume()
    {
      _halted = false;
      Elapsed = 0f;
    }

    /// Get halt state
    public bool Halted
    {
      get { return _halted; }
    }
  }
}