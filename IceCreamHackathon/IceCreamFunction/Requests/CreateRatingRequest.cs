using System;

namespace IceCreamFunction.Requests
{
    public record CreateRatingRequest(
        Guid UserId,
        Guid ProductId,
        string LocationName,
        int Rating,
        string UserNotes);
}