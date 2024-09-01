using System.Net;
using System.Text;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace MotorcycleRental.Api.Tests;

public class MotorcycleEndpointTests
{
    private static readonly TestApplication _application;
    private static readonly HttpClient _client;
    private static readonly IMediator _mediator;

    static MotorcycleEndpointTests()
    {
        try
        {
            _application = new TestApplication();
            _client = _application.CreateClient();
            _mediator = _application.Services.GetRequiredService<IMediator>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Initialization error: " + ex.Message);
            throw;
        }
    }


    [Fact]
    public async Task CreateMotorcycle_ShouldReturnOk()
    {
        // Arrange 
        _mediator.Send(Arg.Any<MotorcycleCreateRequest>()).Returns(await Task.FromResult(new MotorcycleCreateResponse(Guid.NewGuid())));

        // Act
        var createMotorcycleCommand = new MotorcycleCreateRequest(2024, "Yamaha YZF-R3","ABC1234");

        var content = new StringContent(JsonConvert.SerializeObject(createMotorcycleCommand), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/v1/motorcycles", content);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await _mediator.Received().Send(Arg.Any<MotorcycleCreateResponse>());
    }

    // [Fact]
    // public async Task GetAllMotorcycles_ShouldReturnOkWithMotorcycleList()
    // {
    //     // Arrange
    //     var mockMotorcycleResponse1 = new MotorcycleResponse(1, 2024, "Yamaha YZF-R3", "ABC1234");
    //     var mockMotorcycleResponse2 = new MotorcycleResponse(2, 2023, "Honda CB500F", "XYZ9876");
    //     var mockResponse = new GetAllMotorcyclesResponse(new List<MotorcycleResponse> { mockMotorcycleResponse1, mockMotorcycleResponse2 });
    //
    //     _mediator.Send(Arg.Any<GetAllMotorcyclesQuery>()).Returns(Task.FromResult(mockResponse));
    //
    //     // Act
    //     var response = await _client.GetAsync("api/v1/motorcycles");
    //
    //     // Assert
    //     response.Should().NotBeNull();
    //     var motorcycles = JsonConvert.DeserializeObject<GetAllMotorcyclesResponse>(await response.Content.ReadAsStringAsync());
    //     motorcycles.Should().NotBeNull();
    //     await _mediator.Received().Send(Arg.Any<GetAllMotorcyclesQuery>());
    // }
    //
    // [Fact]
    // public async Task GetMotorcycleByLicensePlate_ShouldReturnOkWithMotorcycle()
    // {
    //     // Arrange
    //     var licensePlate = "ABC1234";
    //     var mockMotorcycleResponse = new GetMotorcycleByLicensePlateResponse(1, 2024, "Yamaha YZF-R3", licensePlate);
    //
    //     _mediator.Send(Arg.Any<GetMotorcycleByLicensePlateQuery>()).Returns(Task.FromResult(mockMotorcycleResponse));
    //
    //     // Act
    //     var response = await _client.GetAsync($"api/v1/motorcycles/{licensePlate}");
    //
    //     // Assert
    //     response.Should().NotBeNull();
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var content = await response.Content.ReadAsStringAsync();
    //     var motorcycle = JsonConvert.DeserializeObject<GetMotorcycleByLicensePlateResponse>(content);
    //
    //     motorcycle.Should().NotBeNull();
    //     motorcycle.Id.Should().Be(1);
    //
    //     await _mediator.Received().Send(Arg.Any<GetMotorcycleByLicensePlateQuery>());
    // }
    //
    // [Fact]
    // public async Task UpdateMotorcycleLicensePlate_ShouldReturnOkWithUpdatedMotorcycle()
    // {
    //     // Arrange
    //     var motorcycleId = 1;
    //     var updateRequest = new UpdateMotorcycleLicensePlateCommand { LicensePlate = "NEW1234" };
    //     var mockUpdatedMotorcycleResponse = new UpdateMotorcycleLicensePlateResponse();
    //
    //     _mediator.Send(Arg.Any<UpdateMotorcycleLicensePlateCommand>()).Returns(Task.FromResult(mockUpdatedMotorcycleResponse));
    //
    //     // Act
    //     var response = await _client.PutAsJsonAsync($"api/v1/motorcycles/{motorcycleId}/license-plate", updateRequest);
    //
    //     // Assert
    //     response.Should().NotBeNull();
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var content = await response.Content.ReadAsStringAsync();
    //     var updatedMotorcycle = JsonConvert.DeserializeObject<UpdateMotorcycleLicensePlateResponse>(content);
    //
    //     updatedMotorcycle.Should().NotBeNull();
    //     await _mediator.Received().Send(Arg.Any<UpdateMotorcycleLicensePlateCommand>());
    // }
    //
    // [Fact]
    // public async Task RemoveMotorcycleByLicensePlate_ShouldReturnNoContent()
    // {
    //     // Arrange
    //     var licensePlate = "ABC1234";
    //
    //     _mediator.Send(Arg.Any<RemoveMotorcycleByLicensePlateCommand>()).Returns(Task.CompletedTask);
    //
    //     // Act
    //     var response = await _client.DeleteAsync($"api/v1/motorcycles/{licensePlate}");
    //
    //     // Assert
    //     response.Should().NotBeNull();
    //     response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    //
    //     await _mediator.Received().Send(Arg.Any<RemoveMotorcycleByLicensePlateCommand>());
    // }
}
