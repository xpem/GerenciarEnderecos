using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    [Index(nameof(Id), nameof(UserId))]
    public record Address : BaseModel
    {
        [MaxLength(11)]
        public required string CEP { get; set; }

        [MaxLength(200)]
        public required int Number { get; set; }

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

        public required int UserId { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
