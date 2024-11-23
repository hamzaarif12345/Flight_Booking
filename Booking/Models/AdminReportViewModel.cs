using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Booking.Models
{
    public class AdminReportViewModel
    {
        public int TotalTicketsSold { get; set; }
        public decimal TotalTicketCost { get; set; }
        public decimal AverageTicketCost { get; set; }
        public decimal HighestTicketCost { get; set; }
        public decimal LowestTicketCost { get; set; }
        public int TotalPassengers { get; set; }
        public decimal AveragePassengers { get; set; }
        public double LongestDuration { get; set; }
        public double ShortestDuration { get; set; }
        public int TotalFlights { get; set; }
        public int TotalUsers { get; set; }
    }
}