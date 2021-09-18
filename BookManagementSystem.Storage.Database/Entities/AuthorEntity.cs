using System.Collections.Generic;

namespace BookManagementSystem.Storage.Database.Entities
{
    public class AuthorEntity : BaseEntity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}