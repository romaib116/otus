﻿using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests 
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnerController;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnerController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        public Partner CreateBasePartner()
        {
            var partner = new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "Суперигрушки",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                        CreateDate = new DateTime(2020, 07, 9),
                        EndDate = new DateTime(2020, 10, 9),
                        Limit = 100
                    }
                }
            };

            return partner;
        }
        
        [Fact]
        public async Task SetPartnerPromoCodeLimit_WhenPartnerNotFound_Then404()
        {
            //Arrange
            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(null as Partner);

            var controller = new PartnersController(_partnersRepositoryMock.Object);
            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            //Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(It.IsAny<Guid>(), request);


            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimit_WhenPartnerNotActive_Then400()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = fixture.Build<Partner>()
                .With(x => x.IsActive, false)
                .Without(x => x.PartnerLimits)
                .Create();

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            var result = await _partnerController.SetPartnerPromoCodeLimitAsync(partner.Id, request);


            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimit_WhenPartnerHasIssueNumbersWithoutCancelDateLimit_ThenPromocodeReset()
        {
            // Arrange
            var fixture = new Fixture();
            var issueNumbers = new Random().Next(1, 500);

            var partner = CreateBasePartner();
            partner.NumberIssuedPromoCodes = issueNumbers;

            var limit = fixture.Build<PartnerPromoCodeLimit>()
                .With(x => x.Partner, partner)
                .Without(x => x.CancelDate)
                .Create();

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            await _partnerController.SetPartnerPromoCodeLimitAsync(partner.Id, request);
            var result = await _partnersRepositoryMock.Object.GetByIdAsync(It.IsAny<Guid>());


            //Assert
            result.NumberIssuedPromoCodes.Should().Be(0);
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimit_WhenLimitDoesntHaveCancelDate_ThenFillCancelDate()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            var limit = fixture.Build<PartnerPromoCodeLimit>()
                .With(x => x.Partner, partner)
                .Without(x => x.CancelDate)
                .Create();

            partner.PartnerLimits.Add(limit);

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            await _partnerController.SetPartnerPromoCodeLimitAsync(partner.Id, request);
            var result = await _partnersRepositoryMock.Object.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.PartnerLimits.FirstOrDefault(x => x.CancelDate.HasValue)
                .Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SetPartnerPromoCodeLimit_WhenLimitLessEqualsZero_ThenBadRequest(int limitValue)
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Build<SetPartnerPromoCodeLimitRequest>()
                .With(x => x.Limit, limitValue).Create();

            // Act
            var result = await _partnerController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimit_WhenNewLimit_ThenSavedInDatabase()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            await _partnerController.SetPartnerPromoCodeLimitAsync(partner.Id, request);
            var result = await _partnersRepositoryMock.Object.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.PartnerLimits.FirstOrDefault(x => !x.CancelDate.HasValue).Should().NotBeNull();
        }
    }
}