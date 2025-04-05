using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs
{
    public class TransactionWriteDto
    {
        [Required]
        public int WalletId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("Debit|Credit")]
        public string Type { get; set; }

    }
}
