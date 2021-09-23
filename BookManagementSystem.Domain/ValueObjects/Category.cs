using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ValueOf;

namespace BookManagementSystem.Domain.ValueObjects
{
    [JsonConverter(typeof(CategoryJsonConverter))]
    public class Category:ValueOf<int, Category>
    {
        private readonly ICategoryValidator _categoryValidator;

        public Category(int value, ICategoryValidator categoryValidator) : base(value)
        {
            _categoryValidator = categoryValidator;
        }

        public override async Task<bool> Validate(int value)
        {
            return await _categoryValidator.Validate(value, this);
        }

        public override async Task<Exception> OnInValid(int value)
        {
            return new ArgumentException($"{value} is not a valid category id");
        }
    }
}
