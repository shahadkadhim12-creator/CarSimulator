using DataLogicLibrary.DTO;
using DataLogicLibrary.Factories;
using DataLogicLibrary.Infrastructure.Enums;
using DataLogicLibrary.Services;
using Xunit;

namespace CarSimulatorTest
{
    public class BoundaryTests
    {
        [Fact]
        public void Test32_Exit_ShowsAbortedSimulationMessage()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetCurrentActionMessage(7, 20, "Kalle");

            Assert.Equal("Aborted simulation", result);
        }

        [Fact]
        public void Test38_GasCannotBecomeNegative()
        {
            var service = new SimulationLogicService(
                new DataLogicLibrary.DirectionStrategies.DirectionContext(),
                movementAction =>
                {
                    return movementAction switch
                    {
                        MovementAction.Left => new DataLogicLibrary.DirectionStrategies.TurnLeftStrategy(),
                        MovementAction.Right => new DataLogicLibrary.DirectionStrategies.TurnRightStrategy(),
                        MovementAction.Forward => new DataLogicLibrary.DirectionStrategies.DriveForwardStrategy(),
                        MovementAction.Backward => new DataLogicLibrary.DirectionStrategies.ReverseStrategy(),
                        _ => throw new System.ArgumentException()
                    };
                });

            var status = new StatusDTO
            {
                CardinalDirection = CardinalDirection.North,
                MovementAction = MovementAction.Forward,
                GasValue = 0,
                EnergyValue = 20
            };

            var result = service.DecreaseStatusValues(3, status);

            Assert.Equal(0, result.GasValue);
        }

        [Fact]
        public void Test39_EnergyCannotBecomeNegative()
        {
            var service = new SimulationLogicService(
                new DataLogicLibrary.DirectionStrategies.DirectionContext(),
                movementAction =>
                {
                    return movementAction switch
                    {
                        MovementAction.Left => new DataLogicLibrary.DirectionStrategies.TurnLeftStrategy(),
                        MovementAction.Right => new DataLogicLibrary.DirectionStrategies.TurnRightStrategy(),
                        MovementAction.Forward => new DataLogicLibrary.DirectionStrategies.DriveForwardStrategy(),
                        MovementAction.Backward => new DataLogicLibrary.DirectionStrategies.ReverseStrategy(),
                        _ => throw new System.ArgumentException()
                    };
                });

            var status = new StatusDTO
            {
                CardinalDirection = CardinalDirection.North,
                MovementAction = MovementAction.Forward,
                GasValue = 20,
                EnergyValue = 0
            };

            var result = service.DecreaseStatusValues(3, status);

            Assert.Equal(0, result.EnergyValue);
        }

        [Fact]
        public void Test40_UIStatus_ContainsCorrectCurrentStatus()
        {
            var factory = new StatusFactory();

            var status = factory.CreateStatus();

            Assert.Equal(CardinalDirection.North, status.CardinalDirection);
            Assert.Equal(20, status.GasValue);
            Assert.Equal(20, status.EnergyValue);
        }
    }
}