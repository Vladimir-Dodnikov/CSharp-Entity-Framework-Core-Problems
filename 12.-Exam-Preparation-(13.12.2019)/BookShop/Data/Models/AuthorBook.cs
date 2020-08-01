using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookShop.Data.Models
{
    public class AuthorBook
    {
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        //[JsonIgnore]
        public virtual Author Author { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        //[JsonIgnore]
        public virtual Book Book { get; set; }
    }
}
