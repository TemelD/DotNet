namespace BookStoreAPI.Entities
{
    public class Author
    {
        // Une prop mets a dispostion des accesseurs (getters et setters)
        // ceci est une property
        public int Id { get; set; }
        public required string Firstname { get; set; }

        public required string Lastname { get; set; }
    }
}