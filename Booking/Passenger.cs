//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Booking
{
    using System;
    using System.Collections.Generic;
    
    public partial class Passenger
    {
        public int PassengerId { get; set; }
        public int TicketId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string SeatNumber { get; set; }
    
        public virtual Ticket Ticket { get; set; }
    }
}
