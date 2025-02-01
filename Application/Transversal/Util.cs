using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Transversal
{
    public static class Util
    {
        public static List<ErrorResult> GetErrorResult(this IdentityResult result)
        {
            return result.Errors.Select(e => new ErrorResult(e.Code, e.Description)).ToList();
        }
    }
}
