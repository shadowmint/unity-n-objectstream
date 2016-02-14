using UnityEngine;
using N.Package.Animation;
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
        private SpawnedObjectPool pool;

        /// Time since last spawn
        private float lastSpawn;

        /// How often to spawn objects
        public float spawnObjectEvery;

        /// Total elapsed time
        public float elapsed;

        /// Currently suspeneded
        private bool halted;

        /// The factory callback
        private SpawnCallback<TStream> factory;

        /// Create a new instance
        /// @param template The template GameObject to spawn instances of
        /// @param spawnObjectEvery Spawn a new instance this often
        /// @param maxPoolSize Allow at most this many objects to exist
        /// @param callback Callback to provide animation curve, etc.
        public ObjectStream(Option<GameObject> template, float spawnObjectEvery, int maxPoolSize, SpawnCallback<TStream> factory)
        {
            this.spawnObjectEvery = spawnObjectEvery;
            this.factory = factory;
            pool = new SpawnedObjectPool(template, maxPoolSize);
            elapsed = spawnObjectEvery;
        }

        /// Create a new instance
        /// @param template The template GameObject to spawn instances of
        /// @param spawnObjectEvery Spawn a new instance this often
        /// @param maxPoolSize Allow at most this many objects to exist
        /// @param callback Callback to provide animation curve, etc.
        public ObjectStream(GameObject template, float spawnObjectEvery, int maxPoolSize, SpawnCallback<TStream> factory)
        {
            this.spawnObjectEvery = spawnObjectEvery;
            this.factory = factory;
            pool = new SpawnedObjectPool(Option.Some(template), maxPoolSize);
            elapsed = spawnObjectEvery;
        }

        /// Spawn a new object from the object pool
        /// Add new objects, delete old objects, etc.
        public void Update(float delta)
        {
            if (!halted)
            {
                elapsed += delta;
                if ((elapsed - lastSpawn) > spawnObjectEvery)
                {
                    var req = factory();
                    pool.Instance(req.origin).Then((gp) =>
                    {
                        // Start animation
                        var animation = new FollowPathAnimation(this);
                        animation.pathGroup = req.path;
                        var target = new SingleSpawned(gp);
                        req.manager.Add(req.stream, animation, req.curve, target);
                        lastSpawn = elapsed;

                        req.manager.Events.AddEventHandler<AnimationCompleteEvent>((ep) =>
                        {
                            if (ep.animation == animation)
                            {
                                gp.gameObject.SetActive(false); // return object to pool
                            }
                            else
                            {
                                ep.Api.Keep<AnimationCompleteEvent>();
                            }
                        });
                    });
                }
            }
        }

        /// Halt the object stream, end animations
        /// NB. This stops the stream and resets the object start, it doesn't pause.
        public void Halt()
        { halted = true; }

        /// Resume the object stream from scratch.
        public void Resume()
        {
            halted = false;
            elapsed = 0f;
        }

        /// Get halt state
        public bool Halted
        { get { return halted; } }
    }
}
