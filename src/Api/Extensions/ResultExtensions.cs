using Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return Problem(result);
    }

    public static IActionResult ToActionResult(this Result result, IActionResult? successResult = null)
    {
        if (result.IsSuccess)
            return successResult ?? new NoContentResult();

        return Problem(result);
    }

    private static IActionResult Problem(Result result)
    {
        var status = result.Status switch
        {
            ResultStatus.NotFound => StatusCodes.Status404NotFound,
            ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
            ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
            ResultStatus.Conflict => StatusCodes.Status409Conflict,
            ResultStatus.Validation => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status400BadRequest
        };

        var problem = new ProblemDetails
        {
            Title = result.Error.Code,
            Detail = result.Error.Description,
            Status = status
        };

        return new ObjectResult(problem) { StatusCode = status };
    }
}
