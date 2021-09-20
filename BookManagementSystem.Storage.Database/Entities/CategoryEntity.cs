using System.Collections.Generic;

namespace BookManagementSystem.Storage.Database.Entities
{
    public class CategoryEntity : BaseEntity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }

    }
}
