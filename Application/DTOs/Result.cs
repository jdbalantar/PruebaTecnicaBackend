using System.Net;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class Result<T>
    {
        #region Properties

        public bool IsSuccess { get; }
        public bool OperationHandled { get; } = true;
        public bool ModelErrors { get; } = false;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; } = null;
        private T? _data;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get => _data; set { _data ??= value; } }

        public int StatusCode { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ErrorResult>? Errors { get; } = null;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Paginas { get; private set; } = null;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PaginaActual { get; private set; } = null;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CantidadRegistros { get; private set; } = null;


        #endregion

        #region Constructors
        private Result(HttpStatusCode statusCode, bool isSuccess)
        {
            IsSuccess = isSuccess;
            StatusCode = (int)statusCode;
        }

        private Result(HttpStatusCode statusCode, T? data, bool isSuccess)
        {
            IsSuccess = isSuccess;
            StatusCode = (int)statusCode;
            _data = data;
        }

        private Result(string message, HttpStatusCode statusCode, bool isSuccess)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = (int)statusCode;
        }

        private Result(string message, HttpStatusCode statusCode, bool isSuccess, List<ErrorResult> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = (int)statusCode;
            Errors = errors;
        }
        private Result(string message, HttpStatusCode statusCode, bool isSuccess, List<ErrorResult> errors, bool modelErrors)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = (int)statusCode;
            Errors = errors;
            ModelErrors = modelErrors;
        }

        private Result(string message, HttpStatusCode statusCode, T? Data, bool isSuccess)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = (int)statusCode;
            _data = Data;
        }

        #endregion

        #region Level 200


        /// <summary>
        /// Returns a result indicating the operation was successful with a status code of 200 OK.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 200 OK.</returns>
        public static Result<T> Ok() => new(HttpStatusCode.OK, true);

        /// <summary>
        /// Returns a result indicating the operation was successful with a status code of 200 OK and a message.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 200 OK and the specified message.</returns>
        public static Result<T> Ok(string message) => new(message, HttpStatusCode.OK, true);

        /// <summary>
        /// Returns a result indicating the operation was successful with a status code of 200 OK and data.
        /// </summary>
        /// <param name="data">The data to return.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 200 OK and the specified data.</returns>
        public static Result<T> Ok(T? data) => new(HttpStatusCode.OK, data, true);

        /// <summary>
        /// Returns a result indicating the operation was successful with a status code of 200 OK, a message, and data.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <param name="data">The data to return.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 200 OK, the specified message, and data.</returns>
        public static Result<T> Ok(string message, T? data) => new(message, HttpStatusCode.OK, data, true);



        /// <summary>
        /// Returns a result indicating a resource was successfully created with a status code of 201 Created.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 201 Created.</returns>
        public static Result<T> Created() => new(HttpStatusCode.Created, true);

        /// <summary>
        /// Returns a result indicating a resource was successfully created with a status code of 201 Created and a message.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 201 Created and the specified message.</returns>
        public static Result<T> Created(string message) => new(message, HttpStatusCode.Created, true);

        /// <summary>
        /// Returns a result indicating a resource was successfully created with a status code of 201 Created and data.
        /// </summary>
        /// <param name="data">The data to return.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 201 Created and the specified data.</returns>
        public static Result<T> Created(T? data) => new(HttpStatusCode.Created, data, true);

        /// <summary>
        /// Returns a result indicating a resource was successfully created with a status code of 201 Created, a message, and data.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <param name="data">The data to return.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object with status code 201 Created, the specified message, and data.</returns>
        public static Result<T> Created(string message, T? data) => new(message, HttpStatusCode.Created, data, true);




        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>202 Accepted</c>.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful acceptance without additional data.</returns>
        public static Result<T> Accepted() => new(HttpStatusCode.Accepted, true);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>202 Accepted</c> and a custom message.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful acceptance with the provided message.</returns>
        public static Result<T> Accepted(string message) => new(message, HttpStatusCode.Accepted, true);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>202 Accepted</c> and associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful acceptance with the provided data.</returns>
        public static Result<T> Accepted(T? data) => new(HttpStatusCode.Accepted, data, true);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>202 Accepted</c>, a custom message, and associated data.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful acceptance with the provided message and data.</returns>
        public static Result<T> Accepted(string message, T? data) => new(message, HttpStatusCode.Accepted, data, true);



        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>203 Non-Authoritative Information</c>.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful response with non-authoritative information.</returns>
        public static Result<T> NoAuthoritative() => new(HttpStatusCode.NonAuthoritativeInformation, true);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>203 Non-Authoritative Information</c> and a custom message.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful response with non-authoritative information and the provided message.</returns>
        public static Result<T> NoAuthoritative(string message) => new(message, HttpStatusCode.NonAuthoritativeInformation, true);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>203 Non-Authoritative Information</c> and associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful response with non-authoritative information and the provided data.</returns>
        public static Result<T> NoAuthoritative(T? data) => new(HttpStatusCode.NonAuthoritativeInformation, data, true);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>203 Non-Authoritative Information</c>, a custom message, and associated data.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a successful response with non-authoritative information, the provided message, and data.</returns>
        public static Result<T> NoAuthoritative(string message, T? data) => new(message, HttpStatusCode.NonAuthoritativeInformation, data, true);

        #endregion

        #region Level 400

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>400 Bad Request</c>.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a bad request without additional data.</returns>
        public static Result<T> BadRequest() => new(HttpStatusCode.BadRequest, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>400 Bad Request</c> and a custom message.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a bad request with the provided message.</returns>
        public static Result<T> BadRequest(string message) => new(message, HttpStatusCode.BadRequest, false);

        public static Result<T> BadRequest(string message, List<ErrorResult> errors) => new(message, HttpStatusCode.BadRequest, false, errors);
        public static Result<T> BadRequest(string message, List<ErrorResult> errors, bool modelErrors) => new(message, HttpStatusCode.BadRequest, false, errors, modelErrors);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>400 Bad Request</c> and associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a bad request with the provided data.</returns>
        public static Result<T> BadRequest(T? data) => new(HttpStatusCode.BadRequest, data, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>400 Bad Request</c>, a custom message, and associated data.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a bad request with the provided message and data.</returns>
        public static Result<T> BadRequest(string message, T? data) => new(message, HttpStatusCode.BadRequest, data, false);


        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>401 Unauthorized</c>.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating an unauthorized request without additional data.</returns>
        public static Result<T> Unauthorized() => new(HttpStatusCode.Unauthorized, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>401 Unauthorized</c> and a custom message.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating an unauthorized request with the provided message.</returns>
        public static Result<T> Unauthorized(string message) => new(message, HttpStatusCode.Unauthorized, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>401 Unauthorized</c> and associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating an unauthorized request with the provided data.</returns>
        public static Result<T> Unauthorized(T? data) => new(HttpStatusCode.Unauthorized, data, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>401 Unauthorized</c>, a custom message, and associated data.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating an unauthorized request with the provided message and data.</returns>
        public static Result<T> Unauthorized(string message, T? data) => new(message, HttpStatusCode.Unauthorized, data, false);
        public static Result<T> Unauthorized(string message, List<ErrorResult> errors) => new(message, HttpStatusCode.Unauthorized, false, errors);


        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>403 Forbidden</c>.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a forbidden request without additional data.</returns>
        public static Result<T> Forbidden() => new(HttpStatusCode.Forbidden, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>403 Forbidden</c> and a custom message.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a forbidden request with the provided message.</returns>
        public static Result<T> Forbidden(string message) => new(message, HttpStatusCode.Forbidden, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>403 Forbidden</c> and associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a forbidden request with the provided data.</returns>
        public static Result<T> Forbidden(T? data) => new(HttpStatusCode.Forbidden, data, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>403 Forbidden</c>, a custom message, and associated data.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a forbidden request with the provided message and data.</returns>
        public static Result<T> Forbidden(string message, T? data) => new(message, HttpStatusCode.Forbidden, data, false);
        public static Result<T> Forbidden(string message, List<ErrorResult> errors) => new(message, HttpStatusCode.Forbidden, true, errors);


        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>404 Not Found</c>.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a not found response without additional data.</returns>
        public static Result<T> NotFound() => new(HttpStatusCode.NotFound, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>404 Not Found</c> and a custom message.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a not found response with the provided message.</returns>
        public static Result<T> NotFound(string message) => new(message, HttpStatusCode.NotFound, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>404 Not Found</c> and associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a not found response with the provided data.</returns>
        public static Result<T> NotFound(T? data) => new(HttpStatusCode.NotFound, data, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>404 Not Found</c>, a custom message, and associated data.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a not found response with the provided message and data.</returns>
        public static Result<T> NotFound(string message, T? data) => new(message, HttpStatusCode.NotFound, data, false);
        public static Result<T> NotFound(string message, List<ErrorResult> errors) => new(message, HttpStatusCode.NotFound, true, errors);


        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>409 Conflict</c>.
        /// </summary>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a conflict without additional data.</returns>
        public static Result<T> Conflict() => new(HttpStatusCode.Conflict, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>409 Conflict</c> and a custom message.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a conflict with the provided message.</returns>
        public static Result<T> Conflict(string message) => new(message, HttpStatusCode.Conflict, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>409 Conflict</c> and associated data.
        /// </summary>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a conflict with the provided data.</returns>
        public static Result<T> Conflict(T? data) => new(HttpStatusCode.Conflict, data, false);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> object with the HTTP status code <c>409 Conflict</c>, a custom message, and associated data.
        /// </summary>
        /// <param name="message">The custom message to be included in the result.</param>
        /// <param name="data">The data associated with the result.</param>
        /// <typeparam name="T">The type of data to be included in the result.</typeparam>
        /// <returns>A <see cref="Result{T}"/> object indicating a conflict with the provided message and data.</returns>
        public static Result<T> Conflict(string message, T? data) => new(message, HttpStatusCode.Conflict, data, false);


        #endregion

        #region Error

        public static Result<T> Error(string message) => new(message, HttpStatusCode.InternalServerError, false);
        public static Result<T> Error(string message, List<ErrorResult> errors) => new(message, HttpStatusCode.InternalServerError, false, errors);

        #endregion

        #region Methods

        public void AddPaginationData(int paginaActual, int paginas, int cantidadRegistros)
        {
            PaginaActual = paginaActual;
            Paginas = paginas;
            CantidadRegistros = cantidadRegistros;
        }

        #endregion

    }
}
