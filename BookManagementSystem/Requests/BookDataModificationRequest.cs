using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Requests
{
    public record ChangeTitle(string Title);
    public record ChangeDescription(string Description);
    public record ChangeCategory(int CategoryId);
    public record ModifyAuthor(int AuthorId);

}
