using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberBreakfast.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{

    protected IActionResult Problem(List<Error> errors)
    {
        /// lets check that all errrors are validation errors
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {

                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelStateDictionary);

        }

        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            return Problem();
        }



        var firsterror = errors[0];

        var statusCode = firsterror.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError


        };
        // returning the status code and the description
        return Problem(statusCode: statusCode, title: firsterror.Description);


    }



}