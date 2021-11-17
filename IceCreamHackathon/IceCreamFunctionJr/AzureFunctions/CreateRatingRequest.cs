using System;

namespace IceCreamFunctionJr.AzureFunctions
{
    public record CreateRatingRequest(
        Guid UserId,
        Guid ProductId,
        string LocationName,
        int Rating,
        string UserNotes);
}