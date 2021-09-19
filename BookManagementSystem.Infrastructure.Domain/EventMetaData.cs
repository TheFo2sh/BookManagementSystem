using System;

namespace BookManagementSystem.Infrastructure.Domain
{
    public record EventMetaData(string Type, DateTime CreateTime);
}