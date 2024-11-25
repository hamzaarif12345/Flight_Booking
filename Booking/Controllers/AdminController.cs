using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
        // GET: Admin/EditTicket/{id}
        public ActionResult EditTicket(int id)
        {
            // Fetch the ticket based on the id
            var ticket = _context.Tickets.SingleOrDefault(t => t.TicketId == id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            // Pass the ticket to the view
            return View(ticket);
        }

        // POST: Admin/EditTicket/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult EditTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, return to the same view with the ticket data
                return View(ticket);
            }

            // Fetch the existing ticket from the database
            var existingTicket = _context.Tickets.FirstOrDefault(t => t.TicketId == ticket.TicketId);
            if (existingTicket == null)
            {
                return HttpNotFound("Ticket not found.");
            }

            // Update the ticket properties
            existingTicket.TotalPassengers = ticket.TotalPassengers;
            existingTicket.TotalCost = ticket.TotalCost;
            existingTicket.Status = ticket.Status;

            try
            {
                // Save changes to the database
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Ticket updated successfully!";
                return RedirectToAction("ManageTickets");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving changes: " + ex.Message);
                return View(ticket);
            }
        }


        // POST: Admin/EditFlight/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFlight(Flight updatedFlight)
        {
            if (!ModelState.IsValid)
            {
                // Return the form with validation messages if the model is invalid
                return View(updatedFlight);
            }

            // Find the existing flight in the database
            var flightIn_context = _context.Flights.SingleOrDefault(f => f.FlightId == updatedFlight.FlightId);

            if (flightIn_context == null)
            {
                return HttpNotFound();
            }

            // Update flight properties
            flightIn_context.FlightNumber = updatedFlight.FlightNumber;
            flightIn_context.Origin = updatedFlight.Origin;
            flightIn_context.Destination = updatedFlight.Destination;
            flightIn_context.DepartureTime = updatedFlight.DepartureTime;
            flightIn_context.ArrivalTime = updatedFlight.ArrivalTime;
            flightIn_context.Price = updatedFlight.Price;

            // Save changes to the database
            _context.SaveChanges();

            // Redirect to the ManageFlights page
            return RedirectToAction("ManageFlights");
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

        // GET: Admin/DeleteFlight/5
        public ActionResult DeleteFlight(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }

            // Check for associated tickets
            var hasTickets = _context.Tickets.Any(t => t.FlightId == id);
            ViewBag.HasTickets = hasTickets;

            return View(flight);
        }

        // POST: Admin/DeleteFlight/5
        [HttpPost, ActionName("DeleteFlight")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFlightConfirmed(int id)
        {
            var flight = _context.Flights.Find(id);
            if (flight != null)
            {
                // Check for associated tickets
                var hasTickets = _context.Tickets.Any(t => t.FlightId == id);
                if (hasTickets)
                {
                    TempData["ErrorMessage"] = "Cannot delete the flight as it has associated tickets.";
                    return RedirectToAction("ManageFlights");
                }

                _context.Flights.Remove(flight);
                _context.SaveChanges();
            }

            return RedirectToAction("ManageFlights");
        }

        public ActionResult EditUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return HttpNotFound("User not found.");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == updatedUser.UserId);
            if (existingUser == null)
            {
                return HttpNotFound("User not found.");
            }

            // Update the user's role and status
            existingUser.Role = updatedUser.Role;
            existingUser.IsActive = updatedUser.IsActive;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("ManageUsers");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving changes: " + ex.Message);
                return View(updatedUser);
            }
        }
        public ActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return HttpNotFound("User not found.");
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(int id) // Ensure the parameter name is 'id'
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return HttpNotFound("User not found.");
            }

            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction("ManageUsers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user: " + ex.Message;
                return RedirectToAction("ManageUsers");
            }
        }



        public ActionResult Report()
        {
            var report = new Booking.Models.AdminReportViewModel
            {
                // Total Tickets Sold
                TotalTicketsSold = _context.Tickets.Count(),

                // Total Ticket Cost
                TotalTicketCost = _context.Tickets.Sum(t => (decimal?)t.TotalCost) ?? 0,

                // Average Ticket Cost
                AverageTicketCost = _context.Tickets.Average(t => (decimal?)t.TotalCost) ?? 0,

                // Highest Ticket Cost
                HighestTicketCost = _context.Tickets.Max(t => (decimal?)t.TotalCost) ?? 0,

                // Lowest Ticket Cost
                LowestTicketCost = _context.Tickets.Min(t => (decimal?)t.TotalCost) ?? 0,

                // Total Number of Passengers
                TotalPassengers = _context.Tickets.Sum(t => (int?)t.TotalPassengers) ?? 0,

                // Average Number of Passengers per Ticket
                AveragePassengers = _context.Tickets.Average(t => (decimal?)t.TotalPassengers) ?? 0,

                // Longest Flight Duration
                LongestDuration = _context.Flights.Max(f => (double?)f.Duration) ?? 0,

                // Shortest Flight Duration
                ShortestDuration = _context.Flights.Min(f => (double?)f.Duration) ?? 0,

                // Total Number of Flights
                TotalFlights = _context.Flights.Count(),

                // Total Number of Users
                TotalUsers = _context.Users.Count()
            };

            return View(report);
        }



    }
}
