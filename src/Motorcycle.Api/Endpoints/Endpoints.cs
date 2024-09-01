using MediatR;
using Microsoft.AspNetCore.Mvc;
using Motorcycle.Api.Extensions;
using Motorcycle.Core.Services;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;

namespace Motorcycle.Api.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/auth", async (IUserService userService, UserAuthRequest user) =>
            {
                try
                {
                    var token = await userService.GenerateToken(user.Email, user.Password, CancellationToken.None);
                    return Results.Ok(new { Token = token });
                }
                catch
                {
                    return Results.Unauthorized();
                }
            })
            .WithDisplayName("token user")
            .WithTags("auth");

        app.MapPost("/v1/users", async (IMediator mediator, UserCreateRequest request)
                => await mediator.SendCommand(request))
            .WithDisplayName("create user")
            .WithTags("users")
            .Produces(201, typeof(UserCreateResponse));

        app.MapGet("/v1/users", async (IMediator mediator)
                => await mediator.SendCommand(new UserGetAllRequest()))
            .WithDisplayName("get users")
            .WithTags("users")
            .Produces(200, typeof(UserGetAllResponse));

        app.MapPost("/v1/motorcycles", async (IMediator mediator, MotorcycleCreateRequest request)
                => await mediator.SendCommand(request))
            .WithDisplayName("create motorcycles")
            .WithTags("motorcycles")
            .Produces(201, typeof(MotorcycleCreateResponse))
            .RequireAuthorization("AdminOnly");

        app.MapGet("/v1/motorcycles", async (IMediator mediator, [FromQuery] string? licencePlate)
                => await mediator.SendCommand(new MotorcycleGetAllRequest(licencePlate)))
            .WithDisplayName("get motorcycles")
            .WithTags("motorcycles")
            .Produces(200, typeof(MotorcycleGetAllResponse))
            .RequireAuthorization("AdminOnly");

        app.MapPut("/v1/motorcycles/{id}/licenseplate",
                async (IMediator mediator, string id, MotorcycleLicensePlateUpdateRequest request)
                    => await mediator.SendCommand(request))
            .WithDisplayName("update motorcycles")
            .WithTags("motorcycles")
            .Produces(200, typeof(MotorcycleLicensePlateUpdateResponse));

        app.MapDelete("/v1/motorcycles/{id:guid}", async (IMediator mediator, Guid id)
                => await mediator.SendCommand(new MotorcycleDeleteRequest(id)))
            .WithDisplayName("delete motorcycles")
            .WithTags("motorcycles")
            .Produces(200)
            .RequireAuthorization("AdminOnly");

        app.MapPost("/v1/deliverers", async (IMediator mediator, DelivererCreateRequest request)
                => await mediator.SendCommand(request))
            .WithDisplayName("create deliverer")
            .WithTags("deliverers")
            .Produces(201, typeof(DelivererCreateResponse));

        app.MapGet("/v1/deliverers", async (IMediator mediator, [FromQuery] string? cnh)
                => await mediator.SendCommand(new DelivererGetAllRequest(cnh)))
            .WithDisplayName("get deliverers")
            .WithTags("deliverers")
            .Produces(200, typeof(DelivererGetAllResponse));

        app.MapPatch("/v1/deliverers/{id:guid}/cnhphoto",
                [IgnoreAntiforgeryToken] async (IMediator mediator, Guid id, HttpRequest request)
                    => await mediator.SendCommand(new DeliveryUploadPhotoCnhRequest(id, request.Form.Files[0])))
            .WithDisplayName("update deliverer")
            .WithTags("deliverers")
            .Produces(200, typeof(DeliveryUploadPhotoCnhResponse));

        app.MapPost("/v1/rentals", async (IMediator mediator, MotorcycleRentalRequest request)
                => await mediator.SendCommand(request))
            .WithDisplayName("rental plan")
            .WithTags("rentals")
            .Produces(201, typeof(MotorcycleRentalResponse));

        app.MapPut("/v1/rentals/{id:guid}/enddate",
                async (IMediator mediator, Guid id, MotorcycleRentChangeEndDateRequest request)
                    => await mediator.SendCommand(request))
            .WithDisplayName("rental plan change end date")
            .WithTags("rentals")
            .Produces(201, typeof(MotorcycleRentChangeEndDateResponse));

        app.MapGet("/v1/rentals", async (IMediator mediator)
                => await mediator.SendCommand(new MotorcycleRentalGetAllRequest()))
            .WithDisplayName("get all motorcycle rental")
            .WithTags("rentals")
            .Produces(201, typeof(MotorcycleRentalGetAllResponse));

        app.MapGet("/v1/rentals-plans", async (IMediator mediator)
                => await mediator.SendCommand(new RentalPlanGetAllRequest()))
            .WithDisplayName("get all rental plan")
            .WithTags("rentals")
            .Produces(201, typeof(RentalPlanGetAllResponse));
    }
}