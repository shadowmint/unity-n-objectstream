using UnityEngine;
using N.Package.Animation;
using N.Package.Animation.Animations;
using N.Package.Animation.Curves;
using N.Package.ObjectStream;
using N.Package.ObjectStream.Paths;
using N;

public class LinearStreamTest : MonoBehaviour
{
    public GameObject template;

    public GameObject target;

    public float spawnRate;

    public float lifeTime;

    private ObjectStream<Streams> streams;

    public void Start()
    {
        streams = new ObjectStream<Streams>(template, spawnRate, 16, () =>
        {
            var path = new PathList();
            path.Add(new LinearPath { origin = this.gameObject, target = target });
            path.Add(new FadeOutPath { offset = 0.5f });
            return new SpawnData<Streams>
            {
                stream = Streams.STREAM_0,
                manager = AnimationManager.Default,
                path = path,
                origin = new NTransform(target),
                curve = new Linear(lifeTime)
            };
        });
    }

    public void Update()
    {
        streams.Update(Time.deltaTime);
    }
}
