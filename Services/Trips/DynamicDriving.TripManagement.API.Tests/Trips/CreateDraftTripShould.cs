using System;
using System.Threading;
using System.Threading.Tasks;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.ResultModels;
using DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using DynamicDriving.TripManagement.Domain.TripsAggregate.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DynamicDriving.TripManagement.API.Tests.Trips;

public class CreateDraftTripShould
{
    [Theory]
    [CustomDataSource]
    public async Task Return_success_result_when_model_is_valid(
        [Frozen] Mock<IMediator> mediatorMock,
        DraftTripDto draftTripDto,
        CreateDraftTripModel model,
        TripsController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.IsAny<CreateDraftTrip>(), CancellationToken.None))
            .ReturnsAsync(ResultModel.Ok(draftTripDto));

        // Act
        var actionResult = await sut.CreateDraftTrip(model);

        // Assert
        var envelopeResult = (EnvelopeResult)actionResult;
        envelopeResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        var value = (Envelope<DraftTripDto>)envelopeResult.Envelope;
        value.Result.Id.Should().Be(draftTripDto.Id);
    }

    private class ControllerDataSourceAttribute : CustomDataSourceAttribute
    {
        public ControllerDataSourceAttribute() : base(new MediatorCustomization())
        {
        }

        private class MediatorCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
