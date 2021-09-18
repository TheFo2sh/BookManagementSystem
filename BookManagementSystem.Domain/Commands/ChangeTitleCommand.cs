using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Commands
{
    public record ChangeTitleCommand(Guid Id,string AggregateId,string Title) : BaseCommand(Id, AggregateId);
}
