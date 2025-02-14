using System.ComponentModel;
using ErrorOr;

namespace BuberBreakfast.ServiceErrors;


public static class Errors
{

    public static class Breakfast
    {
        public static Error InvalidName => Error.Validation(
    code: "Breakfast Name length Invalid", description: $"Breakfast name must be at least {Models.Breakfast.MinNameLength} characters long and Maximum {Models.Breakfast.MaxNameLength} characters long"

    );
        public static Error InvalidDescription => Error.Validation(
      code: "Breakfast Description length Invalid", description: $"Description name must be at least {Models.Breakfast.MinNameLength} characters long and Maximum {Models.Breakfast.MaxNameLength} characters long"

      );
        public static Error NotFound => Error.NotFound(code: "Breakfast.NotFound", description: "Breakfast not found");

    }


}