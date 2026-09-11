using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.DTO;
using DataLogicLibrary.Infrastructure.Enums;
using DataLogicLibrary.Services;
using Xunit;

namespace CarSimulatorTest
{
    public class DirectionTests
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
        public void Test4_TurnLeft_NorthBecomesWest()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.North);

            var result = service.PerformAction(1, status);

            Assert.Equal(CardinalDirection.West, result.CardinalDirection);
        }

        [Fact]
        public void Test5_TurnLeft_WestBecomesSouth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.West);

            var result = service.PerformAction(1, status);

            Assert.Equal(CardinalDirection.South, result.CardinalDirection);
        }

        [Fact]
        public void Test6_TurnLeft_SouthBecomesEast()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.South);

            var result = service.PerformAction(1, status);

            Assert.Equal(CardinalDirection.East, result.CardinalDirection);
        }

        [Fact]
        public void Test7_TurnLeft_EastBecomesNorth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.East);

            var result = service.PerformAction(1, status);

            Assert.Equal(CardinalDirection.North, result.CardinalDirection);
        }

        [Fact]
        public void Test8_TurnRight_NorthBecomesEast()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.North);

            var result = service.PerformAction(2, status);

            Assert.Equal(CardinalDirection.East, result.CardinalDirection);
        }

        [Fact]
        public void Test9_TurnRight_EastBecomesSouth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.East);

            var result = service.PerformAction(2, status);

            Assert.Equal(CardinalDirection.South, result.CardinalDirection);
        }

        [Fact]
        public void Test10_TurnRight_SouthBecomesWest()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.South);

            var result = service.PerformAction(2, status);

            Assert.Equal(CardinalDirection.West, result.CardinalDirection);
        }

        [Fact]
        public void Test11_TurnRight_WestBecomesNorth()
        {
            var service = CreateSimulationService();
            var status = CreateStatus(CardinalDirection.West);

            var result = service.PerformAction(2, status);

            Assert.Equal(CardinalDirection.North, result.CardinalDirection);
        }
    }
}