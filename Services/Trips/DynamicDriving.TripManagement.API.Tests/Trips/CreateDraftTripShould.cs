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
    [Theory, CustomDataSource]
    public async Task Return_success_envelope_result_when_model_is_valid(
        [Frozen] Mock<IMediator> mediatorMock,
        DraftTripDto draftTripDto,
        CreateDraftTripRequest request,
        TripsController sut)
    {
        // Arrange
        mediatorMock.Setup(x => x.Send(It.IsAny<CreateDraftTrip>(), CancellationToken.None))
            .ReturnsAsync(ResultModel.Ok(draftTripDto));

        // Act
        var actionResult = await sut.CreateDraftTrip(request);

        // Assert
        var envelopeResult = (EnvelopeResult)actionResult;
        envelopeResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        var resultDto = (DraftTripDto)((Envelope<object>)envelopeResult.Envelope).Result;
        resultDto.Id.Should().Be(draftTripDto.Id);
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
            .ReturnsAsync(ResultModel.Fail<DraftTripDto>(new ErrorResult(ErrorConstants.RecordNotFound, errorMessage)));

        // Act
        var actionResult = await sut.CreateDraftTrip(request);

        // Assert
        var envelopeResult = (EnvelopeResult)actionResult;
        envelopeResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        envelopeResult.Envelope.ErrorCode.Should().Be(ErrorConstants.RecordNotFound);
        envelopeResult.Envelope.ErrorMessage.Should().Be(errorMessage);
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
            .ReturnsAsync(ResultModel.Fail<DraftTripDto>(new ErrorResult(errorCode, errorMessage)));

        // Act
        var actionResult = await sut.CreateDraftTrip(request);

        // Assert
        var envelopeResult = (EnvelopeResult)actionResult;
        envelopeResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        envelopeResult.Envelope.ErrorCode.Should().Be(errorCode);
        envelopeResult.Envelope.ErrorMessage.Should().Be(errorMessage);
    }
}
