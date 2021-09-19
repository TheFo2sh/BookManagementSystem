using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Commands
{
    public record CreateBookCommand(string BookId, string Title, string Description, int CategoryId,
        IEnumerable<int> AuthorsId) : BaseCommand(Guid.NewGuid(), BookId);
}
