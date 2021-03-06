using System;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Commands
{
    public record AddAuthorCommand(Guid Id, string AggregateId, int Author) : BaseCommand(Id, AggregateId);
}