﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Data.Models
{
    public class Seat
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Hall))]
        public int HallId { get; set; }
        public virtual Hall Hall { get; set; }
    }
}
