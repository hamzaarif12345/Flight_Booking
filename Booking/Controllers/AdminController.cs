using System.Linq;
using System.Web.Mvc;
using Booking;


namespace FlightBookingSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AirlineEntities _context;

        public AdminController()
        {
            _context = new AirlineEntities();
        }

        // View all flights
        public ActionResult ManageFlights()
        {
            var flights = _context.Flights.ToList();
            return View(flights);
        }

        // View all users
        public ActionResult ManageUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // View all tickets
        public ActionResult ManageTickets()
        {
            var tickets = _context.Tickets.ToList();
            return View(tickets);
        }

        // Dispose the context
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
