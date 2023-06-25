using AutoFixture;
using Huntr.SecondFactorAuth.BL.Interfaces;
using Huntr.SecondFactorAuth.BL.Models;
using Huntr.SecondFactorAuth.BL.Services;
using Huntr.SecondFactorAuth.Contracts;
using Huntr.SecondFactorAuth.Contracts.Exceptions;
using Huntr.SecondFactorAuth.Contracts.Interfaces;
using Huntr.SecondFactorAuth.Contracts.Request;
using Huntr.SecondFactorAuth.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Huntr.SecondFactorAuth.Tests
{
    public class CodeServiceTests
    {
        private readonly ICodeService _testObject;
        private readonly Mock<IDatabaseServiceFactory> _mockDbServiceFactory;
        private readonly Mock<ICodeGenerationService> _mockCodeGenerationService;
        private readonly Fixture _fixture = new Fixture();

        public CodeServiceTests()
        {
            var dbType = new KeyValuePair<string, string>("DatabaseType", "Mock");
            var codeLifetime = new KeyValuePair<string, string>("Code:LifetimeInMinutes", "5");
            var codeConcurrent = new KeyValuePair<string, string>("Code:NumberOfConcurrentCodePerPhone", "1");

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[] { dbType, codeConcurrent, codeLifetime })
                .Build();

            _mockDbServiceFactory = new Mock<IDatabaseServiceFactory>();
            _mockCodeGenerationService = new Mock<ICodeGenerationService>();
            _testObject = new CodeService(_mockDbServiceFactory.Object, configuration, _mockCodeGenerationService.Object);
        }

        [Fact]
        public async Task SendCodeAsync_WhenProperRequest_ReturnsCode()
        {
            var sendCodeRequest = _fixture.Create<SendCodeRequest>();

            var mockDbService = new Mock<IDatabaseService>();
            mockDbService.Setup(x => x.GetCodesAsync(It.IsAny<GetCodeRequest>()))
                .ReturnsAsync((List<ConfirmationCode>)null);
            mockDbService.Setup(x => x.SaveCodeAsync(It.IsAny<SaveCodeRequest>()))
                .ReturnsAsync((SaveCodeRequest request) =>
                {
                    var differenceInMinutes = request.ExpiryTime.Subtract(DateTime.UtcNow).TotalMinutes;
                    return request.PhoneNumber == $"{sendCodeRequest.CountryCode}{sendCodeRequest.PhoneNumber}"
                            && differenceInMinutes >= 4 & differenceInMinutes <= 5
                            && request.Code == "123";
                });

            _mockDbServiceFactory.Setup(x=>x.GetDatabaseService(It.IsAny<string>()))
                .Returns(mockDbService.Object);
            _mockCodeGenerationService.Setup(x => x.GetCode())
                .Returns("123");

            var response = await _testObject.SendCodeAsync(sendCodeRequest);
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task SendCodeAsync_WhenDbServiceConfigMissing_ReturnsException()
        {
            var sendCodeRequest = _fixture.Create<SendCodeRequest>();            

            _mockDbServiceFactory.Setup(x => x.GetDatabaseService(It.IsAny<string>()))
                .Returns((IDatabaseService)null);

            var exception = await Assert.ThrowsAsync<Exception>(() => { return _testObject.SendCodeAsync(sendCodeRequest); });
            Assert.Equal("Invalid Database Type Configuration", exception.Message);
        }

        [Fact]
        public async Task SendCodeAsync_WhenTooManyConcurrentCodes_ReturnsCode()
        {
            var sendCodeRequest = _fixture.Create<SendCodeRequest>();

            var mockDbService = new Mock<IDatabaseService>();
            mockDbService.Setup(x => x.GetCodesAsync(It.IsAny<GetCodeRequest>()))
                .ReturnsAsync(new List<ConfirmationCode> { new ConfirmationCode { Code = "132", PhoneNumber = $"{sendCodeRequest.CountryCode}{sendCodeRequest.PhoneNumber}", ExpiryTime= DateTime.UtcNow.AddMinutes(5)} });

            _mockDbServiceFactory.Setup(x => x.GetDatabaseService(It.IsAny<string>()))
                .Returns(mockDbService.Object);

            var exception = await Assert.ThrowsAsync<TooManyCodesException>(() => { return _testObject.SendCodeAsync(sendCodeRequest); });
            Assert.Equal("Too many active codes for this phone", exception.Message);
        }

        [Fact]
        public async Task VerifyCodeAsync_WhenMatchingCode_ReturnsSuccess()
        {
            var verifyCodeRequest = _fixture.Create<VerifyCodeRequest>();

            var mockDbService = new Mock<IDatabaseService>();
            mockDbService.Setup(x => x.GetCodesAsync(It.IsAny<GetCodeRequest>()))
                .ReturnsAsync(new List<ConfirmationCode> { new ConfirmationCode { Code = verifyCodeRequest.ConfirmationCode, PhoneNumber = $"{verifyCodeRequest.CountryCode}{verifyCodeRequest.PhoneNumber}", ExpiryTime = DateTime.UtcNow.AddMinutes(5) } });

            _mockDbServiceFactory.Setup(x => x.GetDatabaseService(It.IsAny<string>()))
                .Returns(mockDbService.Object);

            var response = await _testObject.VerifyCodeAsync(verifyCodeRequest);
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task VerifyCodeAsync_WhenNotMatchingCode_ReturnsFailure()
        {
            var verifyCodeRequest = _fixture.Create<VerifyCodeRequest>();

            var mockDbService = new Mock<IDatabaseService>();
            mockDbService.Setup(x => x.GetCodesAsync(It.IsAny<GetCodeRequest>()))
                .ReturnsAsync(new List<ConfirmationCode> { new ConfirmationCode { Code = "1", PhoneNumber = $"{verifyCodeRequest.CountryCode}{verifyCodeRequest.PhoneNumber}", ExpiryTime = DateTime.UtcNow.AddMinutes(5) } });

            _mockDbServiceFactory.Setup(x => x.GetDatabaseService(It.IsAny<string>()))
                .Returns(mockDbService.Object);

            var response = await _testObject.VerifyCodeAsync(verifyCodeRequest);
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public async Task VerifyCodeAsync_WhenDbServiceConfigMissing_ReturnsException()
        {
            var verifyCodeRequest = _fixture.Create<VerifyCodeRequest>();

            _mockDbServiceFactory.Setup(x => x.GetDatabaseService(It.IsAny<string>()))
                .Returns((IDatabaseService)null);

            var exception = await Assert.ThrowsAsync<Exception>(() => { return _testObject.VerifyCodeAsync(verifyCodeRequest); });
            Assert.Equal("Invalid Database Type Configuration", exception.Message);
        }
    }
}
