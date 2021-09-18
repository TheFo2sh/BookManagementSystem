using MediatR;

namespace BookManagementSystem.Infrastructure.Domain
{
    public interface ICommandHandler<in T>:IRequestHandler<T,bool> where T :BaseCommand{}
}