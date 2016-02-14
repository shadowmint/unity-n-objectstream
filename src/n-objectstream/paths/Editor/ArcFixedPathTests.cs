#if N_OBJECTSTREAM_TESTS
using N;
using NUnit.Framework;
using UnityEngine;
using N.Package.ObjectStream;
using N.Package.ObjectStream.Tests;
using N.Package.ObjectStream.Paths;

public class ArcFixedPathTests : N.Tests.Test
{
    private FakeCurve curve;
    private GameObject origin;
    private GameObject target;
    private GameObject mobile;

    private ArcFixedPath fixture()
    {
        curve = new FakeCurve { duration = 5f, elapsed = 0f };
        origin = this.SpawnBlank();
        target = this.SpawnBlank();
        mobile = this.SpawnBlank();
        mobile.transform.position = new Vector3(0f, 0f, 0f);
        var instance = new ArcFixedPath { height = 10f, up = new Vector3(0f, 1f, 0f), origin = origin, target = target, speed = 1f };
        return instance;
    }

    [Test]
    public void test_arc_height()
    {
        var transform = new PathTransform();
        var instance = fixture();
        var spawned = new SpawnedObject() { gameObject = mobile };
        target.transform.position = new Vector3(0f, 0f, 10f);

        curve.elapsed = 0f;
        instance.Update(curve, transform, spawned);
        Assert(transform.position == new Vector3(0f, 0f, 0f));

        curve.elapsed = 2.5f;
        curve.Delta = 2.5f;
        instance.Update(curve, transform, spawned);
        Assert((transform.position - new Vector3(0f, 7.071068f, 2.5f)).magnitude < 0.001f);

        curve.elapsed = 5f;
        curve.Delta = 5f;
        instance.Update(curve, transform, spawned);
        Assert(transform.position == new Vector3(0f, 10f, 5f));

        curve.elapsed = 10.0f;
        curve.Delta = 10.0f; // We never apply to the gameObject, so it doesn't step
        instance.Update(curve, transform, spawned);
        Assert((transform.position - new Vector3(0f, 0f, 10f)).magnitude < 0.001f);

        this.TearDown();
    }
}
#endif
