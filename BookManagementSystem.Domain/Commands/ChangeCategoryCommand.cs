using System;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Commands
{
    public record ChangeCategoryCommand(Guid Id, string AggregateId, int Category) : BaseCommand(Id, AggregateId);
}