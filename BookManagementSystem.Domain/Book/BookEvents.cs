using System;
using System.Threading.Tasks;
using BookManagementSystem.Domain.ValueObjects;

namespace BookManagementSystem.Domain.Book
{
    public record BookEvents(DateTime? CreatedDate)
    {
        public record TitleChanged(string Title, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record CategoryChanged(Category CategoryId, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record DescriptionChanged(string Description, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record AuthorAdded(Author AuthorId, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record AuthorRemoved(Author AuthorId, DateTime? CreatedDate) : BookEvents(CreatedDate);

    }
}