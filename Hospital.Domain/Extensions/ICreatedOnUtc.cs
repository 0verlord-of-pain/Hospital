using System;

namespace Hospital.Domain.Extensions
{
    public interface ICreatedOnUtc
    {
        DateTime CreatedOnUtc { get; set; }
    }
}