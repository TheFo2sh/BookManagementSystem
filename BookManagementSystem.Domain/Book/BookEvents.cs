using System;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Book
{
    public record BookEvents(DateTime? CreatedDate)
    {
        public record TitleChanged(string Title, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record CategoryChanged(int CategoryId, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record DescriptionChanged(string Description, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record AuthorAdded(int AuthorId, DateTime? CreatedDate) : BookEvents(CreatedDate);
        public record AuthorRemoved(int AuthorId, DateTime? CreatedDate) : BookEvents(CreatedDate);

      

    }
}