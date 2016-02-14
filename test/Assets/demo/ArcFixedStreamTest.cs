using UnityEngine;
using N.Package.Animation;
using N.Package.Animation.Animations;
using N.Package.Animation.Curves;
using N.Package.ObjectStream;
using N.Package.ObjectStream.Paths;
using N;

public class ArcFixedStreamTest : MonoBehaviour
{
    public GameObject template;

    public GameObject target;

    public float speed;

    public float fadeInOver;

    public float fadeOutAt;

    public float spawnRate;

    public float lifeTime;

    private ObjectStream<Streams> streams;

    public void Start()
    {
        streams = new ObjectStream<Streams>(template, spawnRate, 16, () =>
        {
            var path = new PathList();
            path.Add(new ArcFixedPath { origin = this.gameObject, target = target, speed = speed, height = 2f, up = new Vector3(0f, 1f, 0f) });
            path.Add(new FadeOutDistance { offset = fadeOutAt, target = target });
            path.Add(new FadeInDistance { distance = fadeInOver });
            return new SpawnData<Streams>
            {
                stream = Streams.STREAM_1,
                manager = AnimationManager.Default,
                path = path,
                origin = new NTransform(gameObject),
                curve = new Linear(lifeTime)
            };
        });
    }

    public void Update()
    {
        streams.Update(Time.deltaTime);
    }
}
