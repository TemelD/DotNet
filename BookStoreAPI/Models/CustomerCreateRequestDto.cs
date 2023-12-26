namespace BookStoreAPI.Models
{
    public class CustomerCreateRequestDto
    {
        public string Identifiant { get; init; } = default!;
        public string Firstname { get; init; } = default!;
        public string Lastname { get; init; } = default!;
        public string MailAdress { get; init; } = default!;
    
    }
}