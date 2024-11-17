using BuyMyHouseMortgageApp.Functions;
using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BuyMyHouseMortgageApp.Tests
{
    public class DailyMortgageApplicationBatchFunctionTests
    {
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;
        private readonly Mock<ILogger<DailyMortgageApplicationBatchFunction>> _mockLogger;
        private readonly Mock<IMortgageApplicationRepository> _mockRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly DailyMortgageApplicationBatchFunction _function;

        public DailyMortgageApplicationBatchFunctionTests()
        {
            // Setup mocks
            _mockLoggerFactory = new Mock<ILoggerFactory>();
            _mockLogger = new Mock<ILogger<DailyMortgageApplicationBatchFunction>>();
            _mockRepository = new Mock<IMortgageApplicationRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockEmailService = new Mock<IEmailService>();

            // Setup logger factory
            _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(_mockLogger.Object);

            // Initialize the function with mocked dependencies
            _function = new DailyMortgageApplicationBatchFunction(
                _mockLoggerFactory.Object,
                _mockRepository.Object,
                _mockConfiguration.Object,
                _mockEmailService.Object
            );
        }

        [Fact]
        public async Task Run_WithExpiredApplications_ProcessesAllApplications()
        {
            // Arrange
            var timerScheduleStatus = new ScheduleStatus
            {
                Last = DateTime.UtcNow.AddDays(-1),
                Next = DateTime.UtcNow.AddDays(1),
                LastUpdated = DateTime.UtcNow
            };

            var timerInfo = new TimerInfo
            {
                ScheduleStatus = timerScheduleStatus
            };

            var expiredApplications = new List<MortgageApplication>
            {
                new MortgageApplication
                {
                    Id = 1,
                    ApplicantName = "John Doe",
                    LoanAmount = 250000,
                    ApplicationStatus = "Pending"
                },
                new MortgageApplication
                {
                    Id = 2,
                    ApplicantName = "Jane Smith",
                    LoanAmount = 300000,
                    ApplicationStatus = "Pending"
                }
            };

            _mockRepository.Setup(r => r.GetExpiredMortgageApplicationsAsync())
                .ReturnsAsync(expiredApplications);

            _mockRepository.Setup(r => r.UpdateMortgageApplicationAsync(It.IsAny<MortgageApplication>()))
                .Returns(Task.CompletedTask);

            _mockEmailService.Setup(e => e.SendOfferEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _function.Run(timerInfo);

            // Assert
            _mockRepository.Verify(r => r.GetExpiredMortgageApplicationsAsync(), Times.Once);
            _mockRepository.Verify(r => r.UpdateMortgageApplicationAsync(It.IsAny<MortgageApplication>()), Times.Exactly(2));
            _mockEmailService.Verify(e => e.SendOfferEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Run_WithNoExpiredApplications_ProcessesNothing()
        {
            // Arrange
            var timerScheduleStatus = new ScheduleStatus
            {
                Last = DateTime.UtcNow.AddDays(-1),
                Next = DateTime.UtcNow.AddDays(1),
                LastUpdated = DateTime.UtcNow
            };

            var timerInfo = new TimerInfo
            {
                ScheduleStatus = timerScheduleStatus
            };

            var expiredApplications = new List<MortgageApplication>();

            _mockRepository.Setup(r => r.GetExpiredMortgageApplicationsAsync())
                .ReturnsAsync(expiredApplications);

            // Act
            await _function.Run(timerInfo);

            // Assert
            _mockRepository.Verify(r => r.GetExpiredMortgageApplicationsAsync(), Times.Once);
            _mockRepository.Verify(r => r.UpdateMortgageApplicationAsync(It.IsAny<MortgageApplication>()), Times.Never);
            _mockEmailService.Verify(e => e.SendOfferEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Run_WithRepositoryError_LogsError()
        {
            // Arrange
            var timerScheduleStatus = new ScheduleStatus
            {
                Last = DateTime.UtcNow.AddDays(-1),
                Next = DateTime.UtcNow.AddDays(1),
                LastUpdated = DateTime.UtcNow
            };

            var timerInfo = new TimerInfo
            {
                ScheduleStatus = timerScheduleStatus
            };

            _mockRepository.Setup(r => r.GetExpiredMortgageApplicationsAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _function.Run(timerInfo));
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
    }
}