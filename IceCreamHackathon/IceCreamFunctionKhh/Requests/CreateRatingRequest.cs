using System;

namespace IceCreamFunctionKhh.Requests
{
    public record CreateRatingRequest(
        Guid id,
        Guid UserId,
        Guid ProductId,
        string LocationName,
        int Rating,
        string UserNotes);
}