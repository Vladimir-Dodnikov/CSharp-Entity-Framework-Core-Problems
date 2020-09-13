using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required, Range(0,1)]
        public PurchaseType Type { get; set; }

        [Required]
        public string ProductKey { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }
        public virtual Card Card { get; set; }

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
