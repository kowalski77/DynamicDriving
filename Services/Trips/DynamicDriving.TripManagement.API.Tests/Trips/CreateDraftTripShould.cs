using System.Threading;
using System.Threading.Tasks;
using DynamicDriving.Models;
using DynamicDriving.SharedKernel.Envelopes;
using DynamicDriving.SharedKernel.Results;
using DynamicDriving.TripManagement.API.UseCases.Trips.CreateDraft;
using DynamicDriving.TripManagement.Application.Trips.Commands;
using DynamicDriving.TripManagement.Domain.TripsAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicDriving.TripManagement.API.Tests.Trips;

public class CreateDraftTripShould
{
    [Theory, CustomDataSource]
    public async Task Return_success_envelope_result_when_model_is_valid(
        [Frozen] Mock<IMediator> mediatorMock,
        DraftTripDto draftTripDto,
        CreateDraftTripRequest request,
        TripsController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.IsAny<CreateDraftTrip>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(draftTripDto));

        // Act
        var actionResult = await sut.CreateDraftTrip(request);

        // Assert
        var envelopeResult = (CreatedAtRouteResult)actionResult;
        envelopeResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        var successEnvelope = (SuccessEnvelope<CreateDraftTripResponse>)envelopeResult.Value!;
        successEnvelope.Data.TripId.Should().Be(draftTripDto.Id);
    }

    [Theory, CustomDataSource]
    public async Task Return_error_envelope_result_when_not_found_user(
        [Frozen] Mock<IMediator> mediatorMock,
        string errorMessage,
        CreateDraftTripRequest request,
        TripsController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.IsAny<CreateDraftTrip>(), CancellationToken.None))
            .ReturnsAsync(Result.Fail<DraftTripDto>(new ErrorResult(ErrorConstants.RecordNotFound, errorMessage)));

        // Act
        var actionResult = await sut.CreateDraftTrip(request);

        // Assert
        var envelopeResult = (EnvelopeResult)actionResult;
        envelopeResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        envelopeResult.ErrorEnvelope!.ErrorCode.Should().Be(ErrorConstants.RecordNotFound);
        envelopeResult.ErrorEnvelope.ErrorMessage.Should().Be(errorMessage);
    }

    [Theory, CustomDataSource]
    public async Task Return_error_envelope_result_when_bad_request(
        [Frozen] Mock<IMediator> mediatorMock,
        string errorCode,
        string errorMessage,
        CreateDraftTripRequest request,
        TripsController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.IsAny<CreateDraftTrip>(), CancellationToken.None))
            .ReturnsAsync(Result.Fail<DraftTripDto>(new ErrorResult(errorCode, errorMessage)));

        // Act
        var actionResult = await sut.CreateDraftTrip(request);

        // Assert
        var envelopeResult = (EnvelopeResult)actionResult;
        envelopeResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        envelopeResult.ErrorEnvelope!.ErrorCode.Should().Be(errorCode);
        envelopeResult.ErrorEnvelope.ErrorMessage.Should().Be(errorMessage);
    }
}
