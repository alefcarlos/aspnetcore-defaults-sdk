using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AlefCarlos.AspNetCoreDefaults.WebApi;

public static class ResultExtensions
{
    public static Results<
        Created<TResponse>,
        ValidationProblem,
        Conflict,
        ProblemHttpResult>
        ToCreated<TValue, TResponse>(
            this Result<TValue> result,
            Func<TValue, string> locationBuilder,
            Func<TValue, TResponse> responseBuilder)
    {
        return result.Status switch
        {
            ResultStatus.Ok =>
                TypedResults.Created(
                    locationBuilder(result.Value),
                    responseBuilder(result.Value)),

            ResultStatus.Invalid =>
                CreateValidationProblem(result),

            ResultStatus.Conflict =>
                TypedResults.Conflict(),

            _ =>
                CreateProblem(result, "Create failed")
        };
    }

    public static Results<
        Created,
        ValidationProblem,
        Conflict,
        ProblemHttpResult>
        ToCreated(this Result result)
    {
        return result.Status switch
        {
            ResultStatus.Ok =>
                TypedResults.Created(),

            ResultStatus.Invalid =>
                CreateValidationProblem(result),

            ResultStatus.Conflict =>
                TypedResults.Conflict(),

            _ =>
                CreateProblem(result, "Create failed")
        };
    }

    public static Results<
        Ok<TResponse>,
        ValidationProblem,
        ProblemHttpResult>
        ToOk<TValue, TResponse>(
            this Result<TValue> result,
            Func<TValue, TResponse> responseBuilder)
    {
        return result.Status switch
        {
            ResultStatus.Ok =>
                TypedResults.Ok(responseBuilder(result.Value)),

            ResultStatus.Invalid =>
                CreateValidationProblem(result),

            _ =>
                CreateProblem(result, "Operation failed")
        };
    }

    public static Results<
        Ok,
        ValidationProblem,
        ProblemHttpResult>
        ToOk(this Result result)
    {
        return result.Status switch
        {
            ResultStatus.Ok =>
                TypedResults.Ok(),

            ResultStatus.Invalid =>
                CreateValidationProblem(result),

            _ =>
                CreateProblem(result, "Operation failed")
        };
    }

    public static Results<
        Ok<TResponse>,
        NotFound,
        ValidationProblem,
        ProblemHttpResult>
        ToOkOrNotFound<TValue, TResponse>(
            this Result<TValue> result,
            Func<TValue, TResponse> responseBuilder)
    {
        return result.Status switch
        {
            ResultStatus.Ok =>
                TypedResults.Ok(responseBuilder(result.Value)),

            ResultStatus.NotFound =>
                TypedResults.NotFound(),

            ResultStatus.Invalid =>
                CreateValidationProblem(result),

            _ =>
                CreateProblem(result, "Resource not available")
        };
    }

    public static Results<
        Ok,
        NotFound,
        ValidationProblem,
        ProblemHttpResult>
        ToOkOrNotFound(this Result result)
    {
        return result.Status switch
        {
            ResultStatus.Ok =>
                TypedResults.Ok(),

            ResultStatus.NotFound =>
                TypedResults.NotFound(),

            ResultStatus.Invalid =>
                CreateValidationProblem(result),

            _ =>
                CreateProblem(result, "Resource not available")
        };
    }

    public static Results<
        NoContent,
        NotFound,
        ValidationProblem,
        ProblemHttpResult>
        ToNoContent(this Result result)
    {
        return result.Status switch
        {
            ResultStatus.Ok =>
                TypedResults.NoContent(),

            ResultStatus.NotFound =>
                TypedResults.NotFound(),

            ResultStatus.Invalid =>
                CreateValidationProblem(result),

            _ =>
                CreateProblem(result, "Operation failed")
        };
    }

    private static ValidationProblem CreateValidationProblem<T>(Result<T> result)
    {
        return TypedResults.ValidationProblem(
            result.ValidationErrors
                .GroupBy(x => x.Identifier ?? string.Empty)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()));
    }

    private static ValidationProblem CreateValidationProblem(Result result)
    {
        return TypedResults.ValidationProblem(
            result.ValidationErrors
                .GroupBy(x => x.Identifier ?? string.Empty)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()));
    }

    private static ProblemHttpResult CreateProblem(
        Result result,
        string title)
    {
        return TypedResults.Problem(
            title: title,
            detail: string.Join("; ", result.Errors),
            statusCode: StatusCodes.Status400BadRequest);
    }

    private static ProblemHttpResult CreateProblem<T>(
        Result<T> result,
        string title)
    {
        return TypedResults.Problem(
            title: title,
            detail: string.Join("; ", result.Errors),
            statusCode: StatusCodes.Status400BadRequest);
    }
}