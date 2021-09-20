using System;
using System.Collections.Generic;
using System.Linq;

namespace BookManagementSystem.Storage.Database.Entities
{
    public  class BookEntity: BaseEntity<string>
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; } = new ();
        public virtual ICollection<AuthorEntity> Authors { get; set; }
        public  DateTime? CreatedTime { get; set; }

    }
}