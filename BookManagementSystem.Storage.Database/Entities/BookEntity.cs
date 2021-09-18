using System.Collections.Generic;

namespace BookManagementSystem.Storage.Database.Entities
{
    public  class BookEntity: BaseEntity<string>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; } = new ();
        public virtual ICollection<AuthorEntity> Authors { get; set; }

        public override void CopyTo(BaseEntity<string> destination)
        {
            var dest = (BookEntity)destination;
            dest.Title = Title;
            dest.Description = Description;
            dest.CategoryId = CategoryId;
            dest.Authors = Authors;
        }
    }
}