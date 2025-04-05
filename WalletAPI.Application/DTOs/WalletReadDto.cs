namespace WalletAPI.Application.DTOs
{
    public class WalletReadDto
    {
        public int Id { get; set; }
        public string DocumentId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
