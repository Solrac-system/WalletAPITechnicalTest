using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs
{
    public class WalletWriteDto
    {
        [Required]
        public string DocumentId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }

    }
}
