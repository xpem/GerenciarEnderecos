using Domain.Requests.User;
using System.ComponentModel.DataAnnotations;

namespace Domain.Requests
{
    public record AddressRequest : BaseRequest
    {
        public int? Id { get; set; }

        [MaxLength(8)]
        [MinLength(8)]
        [RegularExpression("([1-9][0-9]*)")]
        public required string CEP { get; set; }

        [MaxLength(5)]
        [RegularExpression("([1-9][0-9]*)")]
        public required string Number { get; set; }

        [MaxLength(200)]
        public required string Street { get; set; }

        [MaxLength(200)]
        public string? Complement { get; set; } = null;

        [MaxLength(200)]
        public required string Neighborhood { get; set; }

        [MaxLength(200)]
        public required string City { get; set; }

        [MaxLength(200)]
        public required string State { get; set; }
    }
}
