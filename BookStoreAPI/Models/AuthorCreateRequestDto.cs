namespace BookStoreAPI.Models
{
    public class AuthorCreateRequestDto
    {
        public string Firstname { get; init; } = default!;

        public string Lastname { get; init; } = default!;
    
    }
}