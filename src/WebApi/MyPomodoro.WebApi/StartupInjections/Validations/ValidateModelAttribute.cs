using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using MyPomodoro.Application.Exceptions;

namespace MyPomodoro.WebApi.StartupInjections.Validations
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new HttpStatusException(context.ModelState.Values.SelectMany(e => e.Errors.Select(x => x.ErrorMessage)).ToList());
            }
        }
    }
}