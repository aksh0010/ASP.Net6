

using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Models;

public class Breakfast
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;
    public const int MinDescriptionLength = 3;
    public const int MaxDescriptionLength = 150;
    public Guid ID { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedTime { get; }
    public List<string> Savory { get; }
    public List<string> Sweet { get; }


    private Breakfast(Guid id,
                     String name,
                     string description,
                     DateTime startDatetime,
                     DateTime endDatetime,
                     DateTime lastModifiedTime,
                     List<string> savory,
                     List<string> sweet)
    {
        //enforce constraints
        ID = id;
        Name = name;
        Description = description;
        StartDateTime = startDatetime;
        EndDateTime = endDatetime;
        LastModifiedTime = lastModifiedTime;
        Savory = savory;
        Sweet = sweet;

    }

    public static ErrorOr<Breakfast> Create(
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        List<string> savory,
        List<string> sweet,
        Guid? id = null)
    {
        List<Error> errors = new();

        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Breakfast.InvalidName);

        }
        if (description.Length is < MinDescriptionLength or > MaxDescriptionLength)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);

        }
        var breakfast = new Breakfast(
        id ?? Guid.NewGuid(),
        name,
        description,
        startDateTime,
        endDateTime,
        DateTime.UtcNow,
        savory,
        sweet);
        if (errors.Count > 0)
        {
            return errors;

        }
        return breakfast;

    }

    internal static ErrorOr<Breakfast> From(CreateBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet);

    }

    internal static ErrorOr<Breakfast> From(Guid id, UpsertBreakfastRequest request)
    {
        return Create(
          request.Name,
          request.Description,
          request.StartDateTime,
          request.EndDateTime,
          request.Savory,
          request.Sweet,
          id);
    }
}
