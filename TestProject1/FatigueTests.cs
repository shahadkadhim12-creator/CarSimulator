using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.DTO;
using DataLogicLibrary.Infrastructure.Enums;
using DataLogicLibrary.Services;
using Xunit;

namespace CarSimulatorTest
{
    public class FatigueTests
    {
        private StatusDTO CreateStatus()
        {
            return new StatusDTO
            {
                CardinalDirection = CardinalDirection.North,
                MovementAction = MovementAction.Forward,
                GasValue = 20,
                EnergyValue = 20
            };
        }

        private SimulationLogicService CreateSimulationService()
        {
            var directionContext = new DirectionContext();

            SimulationLogicService.DirectionStrategyResolver resolver =
                movementAction =>
                {
                    return movementAction switch
                    {
                        MovementAction.Left => new TurnLeftStrategy(),
                        MovementAction.Right => new TurnRightStrategy(),
                        MovementAction.Forward => new DriveForwardStrategy(),
                        MovementAction.Backward => new ReverseStrategy(),
                        _ => throw new System.ArgumentException()
                    };
                };

            return new SimulationLogicService(directionContext, resolver);
        }

        [Fact]
        public void Test26_Driving_AffectsFatigue()
        {
            var service = CreateSimulationService();
            var status = CreateStatus();

            var result = service.DecreaseStatusValues(3, status);

            Assert.True(result.EnergyValue < 20);
        }

        [Fact]
        public void Test27_EnergyAboveWarningLimit_NoWarning()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetDriverStatusMessage(11, "Kalle");

            Assert.Null(result);
        }

        [Fact]
        public void Test28_EnergyAtWarningLimit_ShowsWarning()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetDriverStatusMessage(10, "Kalle");

            Assert.Equal(
                "Kalle is getting tired, consider taking a rest.",
                result);
        }

        [Fact]
        public void Test29_MaxFatigue_ShowsSeriousWarning()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetDriverStatusMessage(0, "Kalle");

            Assert.Equal(
                "Kalle has fallen asleep!!!! WATCH OUT",
                result);
        }

        [Fact]
        public void Test30_Rest_RestoresEnergy()
        {
            var service = CreateSimulationService();
            var status = CreateStatus();
            status.EnergyValue = 5;

            var result = service.PerformAction(5, status);

            Assert.Equal(20, result.EnergyValue);
        }
    }
}