using HM.Application.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HM.API.Utils
{
    public static class ResponsePatternUtil
    {
        public async static Task<IActionResult> BaseResponse<T>(ResponseViewModel<T> model) where T : class
        {
            switch (model.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return await Task.FromResult(new OkObjectResult(model));
                    }

                case HttpStatusCode.BadRequest:
                    {
                        return await Task.FromResult(new BadRequestObjectResult(model));
                    }

                case HttpStatusCode.NoContent:
                    {
                        return await Task.FromResult(new NoContentResult());
                    }

                case HttpStatusCode.UnprocessableEntity:
                    {
                        return await Task.FromResult(new UnprocessableEntityObjectResult(model));
                    }

                case HttpStatusCode.Conflict:
                    {
                        return await Task.FromResult(new ConflictObjectResult(model));
                    }

                case HttpStatusCode.Forbidden:
                    {
                        return await Task.FromResult(new BadRequestObjectResult(model));
                    }

                default:
                    {
                        return await Task.FromResult(new StatusCodeResult(500));
                    }
            }
        }
    }
}
