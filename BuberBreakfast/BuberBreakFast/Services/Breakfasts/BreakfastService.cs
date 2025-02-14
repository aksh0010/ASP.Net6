using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts;


public class BreakfastService : IBreakfastService
{

    private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();

    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        /// Basically we will implement and connect to database to post data there

        // for now we are only doing basic

        _breakfasts.Add(breakfast.ID, breakfast);
        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        _breakfasts.Remove(id);
        return Result.Deleted;
    }



    public ErrorOr<UpsertBreakfast> UpsertBreakfast(Breakfast breakfast)
    {

        var isNewlyCreated = !_breakfasts.ContainsKey(breakfast.ID);
        _breakfasts[breakfast.ID] = breakfast;
        return new UpsertBreakfast(isNewlyCreated);
    }

    ErrorOr<Breakfast> IBreakfastService.GetBreakfast(Guid id)
    {

        if (_breakfasts.TryGetValue(id, out var breakfast))
        {

            return _breakfasts[id];
        }

        return Errors.Breakfast.NotFound;




    }
}