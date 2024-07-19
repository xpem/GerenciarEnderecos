using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record User : BaseModel
    {
        [MaxLength(150)]
        public required string Name { get; set; }

        [MaxLength(250)]
        public required string Email { get; set; }

        [MaxLength(350)]
        public required string Password { get; set; }
    }
}
