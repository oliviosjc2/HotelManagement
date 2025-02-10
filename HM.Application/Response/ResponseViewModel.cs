using FluentValidation.Results;
using System.Net;

namespace HM.Application.Response
{
    public class ResponseViewModel<T> where T : class
    {
        public HttpStatusCode HttpStatusCode { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }
        public List<string> Errors { get; private set; }

        protected ResponseViewModel()
        {
            Errors = new List<string>();
        }

        public static ResponseViewModel<T> GetResponse(HttpStatusCode statusCode, string message)
        {
            return new ResponseViewModel<T>
            {
                HttpStatusCode = statusCode,
                Message = message
            };
        }

        public static ResponseViewModel<T> GetResponse(HttpStatusCode statusCode, string message, T data)
        {
            return new()
            {
                HttpStatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ResponseViewModel<T> GetResponse(ValidationResult validationResult)
        {
            ResponseViewModel<T> response = new();
            if (!validationResult.IsValid)
            {
                response.HttpStatusCode = HttpStatusCode.UnprocessableEntity;
                response.Errors = new List<string>();
                if (validationResult.Errors.Any())
                    response.Errors.Add(validationResult.Errors[0].ErrorMessage);
            }
            else
                response.HttpStatusCode = HttpStatusCode.OK;

            return response;
        }

        internal static ResponseViewModel<string> GetResponse(object badRequest, string v)
        {
            throw new NotImplementedException();
        }

        public static ResponseViewModel<T> GetPagedResponse(
            HttpStatusCode statusCode,
            string message,
            T data)
        {
            return new ResponseViewModel<T>
            {
                HttpStatusCode = statusCode,
                Message = message,
                Data = data
            };
        }
    }
}
