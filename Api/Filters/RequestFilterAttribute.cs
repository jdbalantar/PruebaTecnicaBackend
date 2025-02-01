using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Api.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class RequestFilterAttribute(ILogger<RequestFilterAttribute> logger) : Attribute, IActionFilter
    {
        private readonly ILogger<RequestFilterAttribute> logger = logger;

        /// <summary>
        /// Método que se ejecuta cuando se comienza una petición
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                logger.LogWarning("La solicitud contiene errores de validación");
            ValidarModelo(context);
        }

        /// <summary>
        /// Método que se ejecuta cuando finaliza la petición
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context) { }

        private static void ValidarModelo(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            var modelType = context.ActionArguments.Values.FirstOrDefault()?.GetType();
            var errors = context.ModelState
                .Where(x => x.Value!.Errors.Any())
                .SelectMany(x => x.Value!.Errors.Select(y => new
                {
                    Field = GetDisplayName(modelType, x.Key),
                    Error = y.ErrorMessage
                }))
                .GroupBy(e => e.Field)
                .Select(g => new
                {
                    Field = g.Key,
                    Errors = g.Select(e => e.Error).ToList()
                })
                .ToList();

            context.Result = new BadRequestObjectResult(new
            {
                isSuccess = false,
                operationHandled = true,
                modelErrors = true,
                message = "La solicitud contiene errores de validación.",
                statusCode = 400,
                errors
            });
        }

        /// <summary>
        /// Método auxiliar para obtener el nombre de visualización (Display) de una propiedad
        /// </summary>
        /// <param name="modelType">El tipo del modelo</param>
        /// <param name="propertyName">El nombre de la propiedad</param>
        /// <returns>El nombre de visualización o el nombre de la propiedad si no hay DisplayAttribute</returns>
        private static string GetDisplayName(Type? modelType, string propertyName)
        {
            if (modelType == null)
                return propertyName; // Si no podemos determinar el tipo del modelo, devolver el nombre original

            var property = modelType.GetProperty(propertyName);
            if (property != null)
            {
                var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    return displayAttribute.Name ?? propertyName; // Devolver el nombre del Display o el nombre original
                }
            }

            return propertyName; // Si no hay DisplayAttribute, devolver el nombre original
        }
    }
}
