namespace BookStoreAPI.Entities
{
    public class Book
    {
        // Une prop mets a dispostion des accesseurs (getters et setters)
        // ceci est une property
        public int Id { get; set; }
        public required string Title { get; set; }
        public Author? Author { get; set; }
        public Gender? Gender { get; set; }
        public Editor? Editor { get; set; }
        public Customer? Customer { get; set; }

        public string Abstract { get; set; } = string.Empty;



    }
}