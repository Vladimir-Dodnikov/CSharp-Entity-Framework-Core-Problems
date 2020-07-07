using P03_FootballBetting.Data.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Bet
    {
        //Bet – BetId, Amount, Prediction, DateTime, UserId, GameId
        public int BetId { get; set; }
        public decimal Amount { get; set; }
        public Prediction Prediction { get; set; }
        public DateTime DateTime { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; }  //vitual for supporting LazyLoading
    }
}
