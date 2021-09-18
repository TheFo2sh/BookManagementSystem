using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BookManagementSystem.Domain.Book
{
    public record BookStateChangedNotification(BookState State) : INotification;

}
