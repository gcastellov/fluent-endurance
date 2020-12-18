using System.Threading.Tasks;
using FluentEndurance.Notifications;
using FluentEndurance.Samples.Features;
using FluentEndurance.Samples.NotificationHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace FluentEndurance.Samples
{
    public class MotionTests : BaseTest
    {
        private readonly AutopilotFeature _autopilotFeature;
        private readonly BrakesFeature _brakesFeature;
        private readonly EngineFeature _engineFeature;
        private readonly GearsFeature _gearsFeature;
        private readonly SteeringFeature _steeringFeature;

        public MotionTests(ITestOutputHelper output)
            : base(_ => { }, services =>
            {
                var notificationHandler = new StatusNotificationHandler();
                notificationHandler.Write += output.WriteLine;
                services.AddSingleton<INotificationHandler<StatusNotification>>(notificationHandler);
                services.AddSingleton<INotificationHandler<PerformanceStatusNotification>>(notificationHandler);
            })
        {
            _autopilotFeature = new AutopilotFeature();
            _brakesFeature = new BrakesFeature();
            _engineFeature = new EngineFeature();
            _gearsFeature = new GearsFeature();
            _steeringFeature = new SteeringFeature();
        }

        [Fact]
        public Task CarShouldMakeItsRoutineTwice()
            => UseFeatureSetGroup().For(Times.As(2))
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

    }
}