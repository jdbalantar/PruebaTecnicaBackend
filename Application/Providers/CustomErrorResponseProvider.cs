using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Application.Constants;

namespace Application.Providers
{
    public class CustomErrorResponseProvider : IErrorResponseProvider
    {
        public IActionResult CreateResponse(ErrorResponseContext context)
        {
            var result = Result<string>.BadRequest("La versión de API solicitada no es válida o no está soportada");
            var jsonResponse = JsonConvert.SerializeObject(result, ServerOptions.JsonSettings);
            return new ContentResult
            {
                Content = jsonResponse,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }

}
