using MediatR;
using MediatR.Pipeline;

namespace BookManagementSystem.Infrastructure.Domain
{
    public interface ICommandValidator<in T>:IPipelineBehavior<T,bool> where T:BaseCommand{}
    public interface ICommandHandler<in T>:IRequestHandler<T,bool> where T :BaseCommand{}
}