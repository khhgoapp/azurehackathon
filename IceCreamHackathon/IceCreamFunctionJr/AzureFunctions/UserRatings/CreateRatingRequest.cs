using System;

namespace IceCreamFunctionJr.AzureFunctions.UserRatings
{
    public record CreateRatingRequest(
        Guid UserId,
        Guid ProductId,
        string LocationName,
        int Rating,
        string UserNotes);
}