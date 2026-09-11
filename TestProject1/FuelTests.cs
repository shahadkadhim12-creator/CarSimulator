using DataLogicLibrary.DTO;
using DataLogicLibrary.Infrastructure.Enums;
using DataLogicLibrary.Services;
using Xunit;

namespace CarSimulatorTest
{
    public class FuelTests
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

        [Fact]
        public void Test20_Driving_DecreasesGas()
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

            var status = CreateStatus();

            var result = service.DecreaseStatusValues(3, status);

            Assert.True(result.GasValue < 20);
        }

        [Fact]
        public void Test21_GasZero_CarDoesNotDrive()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetCurrentActionMessage(3, 0, "Kalle");

            Assert.Equal("Refuel to drive the car!", result);
        }

        [Fact]
        public void Test22_GasZero_ShowsWarning()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetCarStatusMessage(0);

            Assert.Equal(
                "Car is completely out of gas! You have to refuel to drive the car!",
                result);
        }

        [Fact]
        public void Test23_Refuel_GasBecomes20()
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

            var status = CreateStatus();
            status.GasValue = 5;

            var result = service.PerformAction(6, status);

            Assert.Equal(20, result.GasValue);
        }

        [Fact]
        public void Test24_GasFive_ShowsAlmostEmptyWarning()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetCarStatusMessage(5);

            Assert.Equal("Car is almost out of gas!", result);
        }

        [Fact]
        public void Test25_GasTen_ShowsLowGasWarning()
        {
            var messageService = new StatusMessageService();

            var result = messageService.GetCarStatusMessage(10);

            Assert.Equal(
                "Car is running out of gas, consider refuelling the car.",
                result);
        }
    }
}