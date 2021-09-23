using System.Threading.Tasks;
using ValueOf;

namespace BookManagementSystem.Domain.ValueObjects
{
    public interface IValueValidator<TValue, TOwner> where TOwner : ValueOf<TValue, TOwner>
    {
        Task<bool> Validate(TValue value, TOwner owner);
    }
    public interface ICategoryValidator: IValueValidator<int,Category>{}
}