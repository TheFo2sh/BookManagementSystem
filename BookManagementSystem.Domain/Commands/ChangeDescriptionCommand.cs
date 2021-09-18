using System;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Commands
{
    public record ChangeDescriptionCommand(Guid Id, string AggregateId, string Description) : BaseCommand(Id, AggregateId);
}