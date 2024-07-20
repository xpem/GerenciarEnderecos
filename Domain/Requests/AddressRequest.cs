using Domain.Requests.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Requests
{
    public record AddressRequest : BaseRequest
    {
        [MaxLength(8)]
        [MinLength(8)]
        public required string CEP { get; set; }

        [MaxLength(5)]
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
