using System.Collections.Immutable;

namespace BookManagementSystem.Domain.Book
{
    public record BookState(string Id,
                            string Title = default,
                            string Description = default,
                            int CategoryId = default,
                            ImmutableList<int> AuthorsId = default);
}
