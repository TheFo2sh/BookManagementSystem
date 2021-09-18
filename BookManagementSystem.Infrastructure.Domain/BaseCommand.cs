using System;
using MediatR;

namespace BookManagementSystem.Infrastructure.Domain
{
    public record BaseCommand(Guid Id,string AggregateId):IRequest<bool>
    {
      
    }
}