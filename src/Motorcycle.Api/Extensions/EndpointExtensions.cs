using System.Net;
using System.Net.Mime;
using MediatR;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Models;
using OperationResult;
using ApplicationException = Motorcycle.Shared.Exceptions.ApplicationException;

namespace Motorcycle.Api.Extensions;

public static class EndpointExtensions
{
    public static async Task<IResult> SendCommand<T>(
        this IMediator mediator,
        IRequest<Result<T>> request,
        Func<T, IResult>? onSuccess = null)
    {
        var result = await mediator.Send(request);
        return HandleResult(result, onSuccess);
    }

    public static async Task<IResult> SendCommand(
        this IMediator mediator,
        IRequest<Result> request,
        Func<IResult>? onSuccess = null)
    {
        var result = await mediator.Send(request);
        return HandleResult(result, onSuccess);
    }

    private static IResult HandleResult<T>(Result<T> result, Func<T, IResult>? onSuccess)
    {
        return result switch
        {
            (true, var response, _) => onSuccess != null ? onSuccess(response!) : Results.Ok(response),
            var (_, _, error) => HandleError(error!)
        };
    }

    private static IResult HandleResult(Result result, Func<IResult>? onSuccess)
    {
        return result switch
        {
            (true, _) => onSuccess != null ? onSuccess() : Results.Ok(),
            var (_, error) => HandleError(error!)
        };
    }

    private static IResult HandleError(Exception error)
    {
        return error switch
        {
            ValidationException e => Results.BadRequest(e.ErrorsDictionary),
            ApplicationException e => new StatusCodeResult<ErrorResponse>(
                (int)HttpStatusCode.UnprocessableEntity,
                new ErrorResponse(e.Message)),
            _ => Results.StatusCode(500)
        };
    }

    private readonly record struct StatusCodeResult<T>(int StatusCode, T? Value) : IResult
    {
        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCode;
            return Value is null
                ? Task.CompletedTask
                : httpContext.Response.WriteAsJsonAsync(Value, Value.GetType(), options: null,
                    contentType: MediaTypeNames.Application.Json);
        }
    }
}