using System;

namespace Hospital.Domain.Extensions
{
    public interface IUpdatedOnUtc
    {
        DateTime? UpdatedOnUtc { get; set; }
    }
}