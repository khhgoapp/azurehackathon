using System;

namespace IceCreamFunctionKhh
{
    public record RatingDocument(
        Guid id,
        Guid UserId,
        Guid ProductId,
        string LocationName,
        int Rating,
        string UserNotes);
}
