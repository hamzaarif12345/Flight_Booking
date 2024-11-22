using System;
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
                _context.Flights.Remove(flight);
                _context.SaveChanges();
            }

            // Redirect to ManageFlights
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




    }
}
