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
    }
    public class BookViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public IEnumerable<string> Authors { get; set; }


    }
}
