using System.Text.Json;
using GistBlog.DAL.Entities.Resources;
using GistBlog.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers;

[ApiController]
public abstract class BaseController : Controller
{
    private readonly ILogger logger;

    protected BaseController(ILogger logger)
    {
        this.logger = logger;
    }

    protected ActionResult<TRes> HandleResponse<TRes>(TRes result) where TRes : StatusResource
    {
        SetResultStatus(result);
        var path = HttpContext.Request.Path;
        var response = JsonSerializer.Serialize(result);

        logger.LogInformation("Request On {Route} returned a response of {Response}", path, response);

        return result.ResponseType switch
        {
            ResponseType.Success => Ok(result),
            ResponseType.Created => Ok(result),
            ResponseType.Unauthorized => Unauthorized(result),
            ResponseType.NoData => NotFound(result),
            ResponseType.Conflict => Conflict(result),
            ResponseType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, result),
            ResponseType.ServiceError => StatusCode(StatusCodes.Status500InternalServerError, result),
            _ => BadRequest(result)
        };
    }

    protected ActionResult<TRes> HandleResponse<TRes>(string actionName, object routeValues, TRes result)
        where TRes : StatusResource
    {
        SetResultStatus(result);
        var path = HttpContext.Request.Path;
        var response = JsonSerializer.Serialize(result);

        logger.LogInformation("Request On {Route} returned a response of {Response}", path, response);
        return result.ResponseType switch
        {
            ResponseType.Created => CreatedAtAction(actionName, routeValues, result),
            _ => HandleResponse(result)
        };
    }

    private static void SetResultStatus<TRes>(TRes result) where TRes : StatusResource
    {
        result.Status = result.ResponseType is ResponseType.Created or ResponseType.Success;
    }
}
