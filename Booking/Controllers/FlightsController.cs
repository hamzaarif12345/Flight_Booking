using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class FlightsController : Controller
    {
        private readonly AirlineEntities _db = new AirlineEntities();

        public ActionResult Index()
        {
            var flights = _db.Flights.ToList();
            return View(flights);
        }

        // Add Edit and Delete actions here as required.
        public ActionResult Edit(int id)
        {
            var flight = _db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Flight flight)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(flight).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flight);
        }
        public ActionResult Delete(int id)
        {
            var flight = _db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            _db.Flights.Remove(flight);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
