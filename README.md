![.NET Core](https://github.com/gcastellov/fluent-endurance/workflows/.NET%20Core/badge.svg)
![Publish NuGet](https://github.com/gcastellov/fluent-endurance/workflows/Publish%20NuGet/badge.svg?branch=main&event=push)

# fluent-endurance
A fluent endurance automation framework.


## Samples
For the sake of providing some examples, the solution contains set of tests which simulates certain car features.

In the following snippet the engine must start within 1200 ms and stop within 800 ms. If any of these two operation reach the timing the execution fails. This set of operations will repeat for 50 times. 

```
[Fact]
public Task EngineShouldStartAndStop()
    => UseFeatureSetGroup(Times.Being(50))
        .WithSet(group => group.Create()
            .WithStep(_engineFeature, (engine, ct) => engine.Start(ct), Timeout.Being(1200))
            .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct), Timeout.Being(800)))
        .Run();
```
Which outputs:
```
Executed engine.Start(ct) taking 1007.4665 ms
Executed engine.Stop(ct) taking 60.9244 ms
Executed engine.Start(ct) taking 1002.506 ms
Executed engine.Stop(ct) taking 58.2176 ms
...
```

Timespans can be used in order to define how long the operations must take instead of repeating them certain times.

```
[Fact]
public Task EngineShouldStartRevAndStopDuringTime()
    => UseFeatureSetGroup(Time.Minutes(1))
        .WithSet(group => group.Create().During(Time.Seconds(20))
            .WithStep(_engineFeature, (engine, ct) => engine.Start(ct))
            .WithStep(_engineFeature, (engine, ct) => engine.Rev3000(ct))
            .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct)))
        .Run();
```

Another more complete sample.

```
[Fact]
public Task CarShouldMakeItsRoutineTwice()
    => UseFeatureSetGroup(Times.Being(2))
        .WithSet(group => group.Create().As("Warm up")
            .WithStep(_engineFeature, (engine, ct) => engine.Start(ct))
            .WithStep(_engineFeature, (engine, ct) => engine.Rev3000(ct)))
        .WithSet(group => group.Create().As("Routine")
            .WithStep(_autopilotFeature, (autopilot, ct) => autopilot.UnPark(ct))
            .WithStep(_gearsFeature, (gears, ct) => gears.ChangeToNeutral(ct))
            .WithStep(_gearsFeature, (gears, ct) => gears.ChangeToDrive(ct)))
        .WithSet(group => group.Create().As("Maneuvers").For(Times.Twice)
            .WithStep(_engineFeature, (engine, ct) => engine.Accelerate100(ct))
            .WithStep(_autopilotFeature, (autopilot, ct) => autopilot.Drive(ct))
            .WithStep(_steeringFeature, (steering, ct) => steering.Left(ct))
            .WithStep(_steeringFeature, (steering, ct) => steering.Forward(ct))
            .WithStep(_steeringFeature, (steering, ct) => steering.Right(ct))
            .WithStep(_steeringFeature, (steering, ct) => steering.Forward(ct))
            .WithStep(_engineFeature, (engine, ct) => engine.Accelerate150(ct))
            .WithStep(_brakesFeature, (brakes, ct) => brakes.BrakeTo50(ct))
            .WithStep(_autopilotFeature, (autopilot, ct) => autopilot.Drive(ct))
            .WithStep(_brakesFeature, (brakes, ct) => brakes.BrakeTo0(ct)))
        .WithSet(group => group.Create().As("Park")
            .WithStep(_engineFeature, (engine, ct) => engine.Stop(ct))
            .WithStep(_autopilotFeature, (autopilot, ct) => autopilot.Park(ct)))
        .Run();
```


## Setup your tests
This library depends on *IMediator* for triggering events in order to provide certain status about performance during the execution.

The library provides a base class *BaseTest* which registers the *IMediator* default implementation and also provides a helper method to easly start defining the tests. Nonetheless, is up to you whether use this class or implement yours depending on your needs.


```
protected BaseTest(Action<IServiceCollection> configureServices)
{
    var hostBuilder = new HostBuilder();

    hostBuilder.ConfigureServices(collection =>
    {
        collection.AddScoped<FeatureSetGroup>();
        collection.AddSingleton<IMediator>(sp => new Mediator(sp.GetService));
        configureServices(collection);
    });

    _host = hostBuilder.Build();
}
```

## Output status

There are two ways of collecting notifications:
* Live notifications by subscribing to these events. 
* Reading the events each feature has triggered at the end of the test execution.


### Live notifications using notification handlers
```
public MotionTests(ITestOutputHelper output)
    : base(services =>
    {
        var notificationHandler = new StatusNotificationHandler();
        notificationHandler.Write += output.WriteLine;
        services.AddSingleton<INotificationHandler<StatusNotification>>(notificationHandler);
        services.AddSingleton<INotificationHandler<PerformanceStatusNotification>>(notificationHandler);
    })
{
    // ...
}
```

### Reading feature events
```
public Task DisposeAsync()
{
    foreach (var notification in _engineFeature.Notifications)
    {
        _output.WriteLine(notification.Content);
    }

    return Task.CompletedTask;
}
```
