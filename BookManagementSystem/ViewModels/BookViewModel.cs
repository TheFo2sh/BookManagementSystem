using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.ViewModels
{
    public class EventViewModel
    {
        public string Event { get; set; }

        public object Args { get; set; }

        public string EventDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
    }
    public class AuthorViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
    }
    public class BookViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public IEnumerable<string> Authors { get; set; }

    }
    public class CreateBookViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public IEnumerable<int> Authors { get; set; }

    }
}
