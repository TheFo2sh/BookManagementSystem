using System;

namespace BookManagementSystem.Domain.Book
{
    public record BookEvents(DateTime? CreatedDate)
    {
        public record ChangeTitle(string Title, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record ChangeCategory(int CategoryId, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record ChangeDescription(string Description, DateTime? CreatedDate) : BookEvents(CreatedDate);

        public record AddAuthor(int AuthorId, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record RemoveAuthor(int AuthorId, DateTime? CreatedDate) : BookEvents(CreatedDate);

    }
}