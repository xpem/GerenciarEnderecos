﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record BaseModel
    {
        public int Id { get; set; }

        public required DateTime CreatedAt { get; set; }
    }
}
