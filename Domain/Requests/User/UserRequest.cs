using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Requests.User
{
    public record UserRequest : UserSessionRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(150, MinimumLength = 4)]
        public required string Name { get; init; }
    }
}
