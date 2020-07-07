using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Color
    {
        public Color()
        {
            this.PrimaryKitTeams = new HashSet<Team>(); //faster loading, initual Icollection
            this.SecondaryKitTeams = new HashSet<Team>();
        }
        public int ColorId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Team> PrimaryKitTeams { get; set; }  //virtual helps EFC loading faster and dynamic
        public virtual ICollection<Team> SecondaryKitTeams { get; set; }
    }
}
