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
        public ActionResult EditTicket(int id)
        {
            // Find the ticket by ID
            var ticket = _context.Tickets.SingleOrDefault(t => t.TicketId == id);

            if (ticket == null)
            {
                return HttpNotFound(); // Return 404 if ticket is not found
            }

            // Pass the ticket to the view
            return View(ticket);
        }

        // POST: Admin/EditTicket/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTicket(Ticket updatedTicket)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedTicket); // Return the form with validation messages
            }

            // Find the existing ticket in the database
            var ticketInDb = _context.Tickets.SingleOrDefault(t => t.TicketId == updatedTicket.TicketId);

            if (ticketInDb == null)
            {
                return HttpNotFound();
            }

            // Update ticket properties
            ticketInDb.TotalPassengers = updatedTicket.TotalPassengers;
            ticketInDb.TotalCost = updatedTicket.TotalCost;
            ticketInDb.Status = updatedTicket.Status;

            // Save changes to the database
            _context.SaveChanges();

            // Redirect to the ManageTickets page
            return RedirectToAction("ManageTickets");
        }

        // GET: Admin/EditFlight/5
        public ActionResult EditFlight(int id)
        {
            // Find the flight by ID
            var flight = _context.Flights.SingleOrDefault(f => f.FlightId == id);

            if (flight == null)
            {
                return HttpNotFound();
            }

            // Pass the flight to the view
            return View(flight);
        }

        // POST: Admin/EditFlight
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFlight(Flight updatedFlight)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedFlight);
            }

            // Find the existing flight in the database
            var flightInDb = _context.Flights.SingleOrDefault(f => f.FlightId == updatedFlight.FlightId);

            if (flightInDb == null)
            {
                return HttpNotFound();
            }

            // Update flight properties
            flightInDb.FlightNumber = updatedFlight.FlightNumber;
            flightInDb.Origin = updatedFlight.Origin;
            flightInDb.Destination = updatedFlight.Destination;
            flightInDb.DepartureTime = updatedFlight.DepartureTime;
            flightInDb.ArrivalTime = updatedFlight.ArrivalTime;
            flightInDb.Price = updatedFlight.Price;

            // Save changes to the database
            _context.SaveChanges();

            // Redirect to ManageFlights page
            return RedirectToAction("ManageFlights");
        }

    }
}
