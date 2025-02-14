using System.ComponentModel;
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;


public class BreakfastsController : ApiController
{


    private readonly IBreakfastService _breakfastService;
    public BreakfastsController(IBreakfastService breakfastService)
    {

        _breakfastService = breakfastService;


    }


    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        var requestToBreakFastResult = Breakfast.From(request);
        if (requestToBreakFastResult.IsError)
        {
            return Problem(requestToBreakFastResult.Errors);

        }

        var breakfast = requestToBreakFastResult.Value;
        var CreateBreakfastResult = _breakfastService.CreateBreakfast(breakfast);
        return CreateBreakfastResult.Match(
            created => CreatedAtGetBreakFast(breakfast),
            error => Problem(error)

        );
        // if (CreateBreakfastResult.IsError)
        // {

        //     return Problem(CreateBreakfastResult.Errors);

        // }
        // return CreatedAtGetBreakFast(breakfast);
    }


    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {

        ErrorOr<Breakfast> getBreakFastResult = _breakfastService.GetBreakfast(id);


        return getBreakFastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)), errors => Problem(errors)
            // function return the value of first param if there is a value else the errors list is passed to problem function in APIcontroller
        );

        /*
                // below we did first error bcoz error instead getBreakFastResult.Error bcoz its list while firsterror is single

                if (getBreakFastResult.IsError && getBreakFastResult.FirstError == Errors.Breakfast.NotFound)
                {

                    return NotFound();

                }
                var breakfast = getBreakFastResult.Value;
                BreakfastResponse response = MapBreakfastResponse(breakfast);

                return Ok(response);*/
    }



    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)

    {
        var requestToBreakFastResult = Breakfast.From(id, request);

        if (requestToBreakFastResult.IsError)
        {

            return Problem(requestToBreakFastResult.Errors);
        }

        var breakfast = requestToBreakFastResult.Value;
        var upsertedBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);

        /// todo : return 201 if new breakfast was created

        return upsertedBreakfastResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakFast(breakfast) : NoContent(),
            errors => Problem(errors)

        );
    }
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {

        var deleteBreakfastResult = _breakfastService.DeleteBreakfast(id);
        return deleteBreakfastResult.Match(deleted => NoContent(), errors => Problem(errors));

    }


    // Mapper function
    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.ID,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedTime,
            breakfast.Savory,
            breakfast.Sweet
        );
    }
    private IActionResult CreatedAtGetBreakFast(Breakfast breakfast)
    {
        return CreatedAtAction(nameof(GetBreakfast),
                               new { id = breakfast.ID },
                              MapBreakfastResponse(breakfast));
    }

}