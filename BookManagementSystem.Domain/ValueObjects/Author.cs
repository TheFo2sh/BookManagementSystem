using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ValueOf;

namespace BookManagementSystem.Domain.ValueObjects
{
    [JsonConverter(typeof(AuthorJsonConverter))]
    public class Author : ValueOf<int, Author>
    {
        private readonly IAuthorValidator _authorValidator;
        public Author(int value, IAuthorValidator authorValidator) : base(value)
        {
            _authorValidator = authorValidator;
        }

        public override async Task<bool> Validate(int value)
        {
            return await _authorValidator.Validate(value,this);
        }

        public override async Task<Exception> OnInValid(int value)
        {
            return new ArgumentException($"{value} is not a valid author id");
        }
    }
}