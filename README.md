# n-objectstream

![Example](https://raw.github.com/shadowmint/unity-n-objectstream/master/media/sample.gif)

This is a simple helper library for batched objects that follow a specific path.

## Usage

Look at the samples. Something like:

    public void Start()
    {
        streams = new ObjectStream<Streams>(template, spawnRate, 16, () =>
        {
            var path = new PathList();
            path.Add(new ArcFixedPath());
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

Objest instances are all spawned from the same prefab; objects are pooled for
reuse once they are discarded.

See the tests in the `Editor/` folder for each class for usage examples.

## Install

From your unity project folder:

    npm init
    npm install shadowmint/unity-n-objectstream --save
    echo Assets/packages >> .gitignore
    echo Assets/packages.meta >> .gitignore

The package and all its dependencies will be installed in
your Assets/packages folder.

## Development

Setup and run tests:

    npm install
    npm install ..
    cd test
    npm install
    gulp

Remember that changes made to the test folder are not saved to the package
unless they are copied back into the source folder.

To reinstall the files from the src folder, run `npm install ..` again.

### Tests

All tests are wrapped in `#if ...` blocks to prevent test spam.

You can enable tests in: `Player settings > Other Settings > Scripting Define Symbols`

The test key for this package is: N_OBJECTSTREAM_TESTS
