using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.DTO;
using DataLogicLibrary.Infrastructure.Enums;
using DataLogicLibrary.Services;
using Xunit;

namespace CarSimulatorTest
{
    public class MovementTests
    {
        private StatusDTO CreateStatus(CardinalDirection direction)
        {
            return new StatusDTO
            {
                CardinalDirection = direction,
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
        public void Test12_Forward_NorthRemainsNorth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.North);

            var result = service.PerformAction(3, status);

            Assert.Equal(CardinalDirection.North, result.CardinalDirection);
        }

        [Fact]
        public void Test13_Forward_SouthRemainsSouth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.South);

            var result = service.PerformAction(3, status);

            Assert.Equal(CardinalDirection.South, result.CardinalDirection);
        }

        [Fact]
        public void Test14_Forward_EastRemainsEast()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.East);

            var result = service.PerformAction(3, status);

            Assert.Equal(CardinalDirection.East, result.CardinalDirection);
        }

        [Fact]
        public void Test15_Forward_WestRemainsWest()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.West);

            var result = service.PerformAction(3, status);

            Assert.Equal(CardinalDirection.West, result.CardinalDirection);
        }

        [Fact]
        public void Test16_Reverse_NorthBecomesSouth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.North);

            var result = service.PerformAction(4, status);

            Assert.Equal(CardinalDirection.South, result.CardinalDirection);
        }

        [Fact]
        public void Test17_Reverse_SouthBecomesNorth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.South);

            var result = service.PerformAction(4, status);

            Assert.Equal(CardinalDirection.North, result.CardinalDirection);
        }

        [Fact]
        public void Test18_Reverse_EastBecomesWest()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.East);

            var result = service.PerformAction(4, status);

            Assert.Equal(CardinalDirection.West, result.CardinalDirection);
        }

        [Fact]
        public void Test19_Reverse_WestBecomesEast()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.West);

            var result = service.PerformAction(4, status);

            Assert.Equal(CardinalDirection.East, result.CardinalDirection);
        }
    }
}