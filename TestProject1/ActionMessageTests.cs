using DataLogicLibrary.Services;
using Xunit;

namespace CarSimulatorTest
{
    public class ActionMessageTests
    {
        private readonly StatusMessageService _messageService =
            new StatusMessageService();

        [Fact]
        public void Test31_Rest_ShowsCorrectMessage()
        {
            var result = _messageService.GetCurrentActionMessage(5, 20, "Kalle");

            Assert.Equal("Kalle takes a rest", result);
        }

        [Fact]
        public void Test33_Forward_ShowsCorrectMessage()
        {
            var result = _messageService.GetCurrentActionMessage(3, 20, "Kalle");

            Assert.Equal("Car drives forward", result);
        }

        [Fact]
        public void Test34_Left_ShowsCorrectMessage()
        {
            var result = _messageService.GetCurrentActionMessage(1, 20, "Kalle");

            Assert.Equal("Car turns and drives to the left", result);
        }

        [Fact]
        public void Test35_Right_ShowsCorrectMessage()
        {
            var result = _messageService.GetCurrentActionMessage(2, 20, "Kalle");

            Assert.Equal("Car turns and drives to the right", result);
        }

        [Fact]
        public void Test36_Reverse_ShowsCorrectMessage()
        {
            var result = _messageService.GetCurrentActionMessage(4, 20, "Kalle");

            Assert.Equal("Car reverses and drives backwards", result);
        }

        [Fact]
        public void Test37_InvalidCommand_IsHandled()
        {
            var result = _messageService.GetCurrentActionMessage(99, 20, "Kalle");

            Assert.Equal("Car is stationary.", result);
        }
    }
}