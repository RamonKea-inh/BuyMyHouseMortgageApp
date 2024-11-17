using BuyMyHouseMortgageApp.Functions;
using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BuyMyHouseMortgageApp.Tests
{
    public class EmailQueueProcessorFunctionTests
    {
        private readonly Mock<ILogger<EmailQueueProcessorFunction>> _mockLogger;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly EmailQueueProcessorFunction _function;

        public EmailQueueProcessorFunctionTests()
        {
            _mockLogger = new Mock<ILogger<EmailQueueProcessorFunction>>();
            _mockEmailService = new Mock<IEmailService>();
            _function = new EmailQueueProcessorFunction(_mockLogger.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task Run_WithValidQueueMessage_SendsEmail()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                RecipientName = "John Doe",
                OfferDetails = "Special offer details"
            };
            var queueMessage = JsonConvert.SerializeObject(emailMessage);

            _mockEmailService.Setup(e => e.SendOfferEmailAsync(emailMessage.RecipientName, emailMessage.OfferDetails))
                .Returns(Task.CompletedTask);

            // Act
            await _function.Run(queueMessage);

            // Assert
            _mockEmailService.Verify(e => e.SendOfferEmailAsync(emailMessage.RecipientName, emailMessage.OfferDetails), Times.Once);
            _mockLogger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Offer email sent to: John Doe")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task Run_WithInvalidQueueMessage_LogsError()
        {
            // Arrange
            var invalidQueueMessage = "Invalid message";

            // Act
            await _function.Run(invalidQueueMessage);

            // Assert
            _mockEmailService.Verify(e => e.SendOfferEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to deserialize queue message to EmailMessage")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}