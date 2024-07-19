using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses.User
{
    public record UserResponse
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? Email { get; init; }

        public DateTime CreatedAt { get; init; }
    }
}
