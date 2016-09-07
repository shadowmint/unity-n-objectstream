#if N_OBJECTSTREAM_TESTS
using N;
using N.Package.Core;
using N.Package.Core.Tests;
using NUnit.Framework;
using UnityEngine;
using N.Package.ObjectStream;
using N.Package.ObjectStream.Tests;
using N.Package.ObjectStream.Paths;

public class LinearFixedPathTests : TestCase
{
  private FakeCurve curve;
  private GameObject origin;
  private GameObject target;
  private GameObject mobile;

  private LinearFixedPath fixture()
  {
    curve = new FakeCurve {duration = 5f, elapsed = 0f};
    origin = this.SpawnBlank();
    target = this.SpawnBlank();
    mobile = this.SpawnBlank();
    mobile.transform.position = new Vector3(0f, 0f, 0f);
    var instance = new LinearFixedPath {origin = origin, target = target, speed = 1f};
    return instance;
  }

  [Test]
  public void test_fixed_path()
  {
    var transform = new PathTransform();
    var instance = fixture();
    var spawned = new SpawnedObject() {GameObject = mobile};
    target.transform.position = new Vector3(0f, 0f, 10f);
    curve.elapsed = 0f;
    instance.Update(curve, transform, spawned);

    Assert(transform.position == new Vector3(0f, 0f, 0f));


    curve.elapsed = 5f;
    curve.Delta = 5f;
    instance.Update(curve, transform, spawned);
    Assert(transform.position == new Vector3(0f, 0f, 5f));

    this.TearDown();
  }

  [Test]
  public void test_fixed_path_with_moving_target()
  {
    var transform = new PathTransform();
    var instance = fixture();
    var spawned = new SpawnedObject() {GameObject = mobile};
    target.transform.position = new Vector3(0f, 0f, 10f);
    curve.elapsed = 0f;
    instance.Update(curve, transform, spawned);

    Assert(transform.position == new Vector3(0f, 0f, 0f));

    curve.elapsed = 5f;
    curve.Delta = 5f;
    instance.Update(curve, transform, spawned);
    Assert(transform.position == new Vector3(0f, 0f, 5f));

    this.TearDown();
  }
}
#endif