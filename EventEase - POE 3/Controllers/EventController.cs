using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using System.Linq;

namespace EventEase.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index(string eventType, string searchName, DateTime? dateFrom, DateTime? dateTo, bool? isVenueAvailable)
        {
            var query = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventsType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(eventType))
            {
                query = query.Where(e => e.EventType == eventType);//Event Type
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(e => e.EventName.Contains(searchName));//Name of Event
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(e => e.EventDate >= dateFrom.Value.Date);//From Date
            }

            if (dateTo.HasValue)
            {
                var endDate = dateTo.Value.Date.AddDays(1); // to include the full end date
                query = query.Where(e => e.EventDate < endDate);
            }

            if (isVenueAvailable.HasValue)
            {
                query = query.Where(e => e.Venue != null && e.Venue.IsAvailable == isVenueAvailable.Value);//Venue
            }

            var eventTypes = await _context.Events
                .Select(e => e.EventType)
                .Where(e => e != null)
                .Distinct()
                .ToListAsync();

            ViewBag.EventTypes = new SelectList(eventTypes, eventType);
            ViewBag.SelectedEventType = eventType;
            ViewBag.CurrentSearch = searchName;
            ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");
            ViewBag.IsVenueAvailable = isVenueAvailable;

            return View(await query.ToListAsync());
        }




        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.VenueID = new SelectList(_context.Venues, "VenueID", "VenueName");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Events events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VenueID = new SelectList(_context.Venues, "VenueID", "VenueName", events.VenueID);
            return View(events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var events = await _context.Events.FindAsync(id);
            if (events == null)
                return NotFound();

            ViewBag.VenueID = new SelectList(_context.Venues, "VenueID", "VenueName", events.VenueID);
            return View(events);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Events events)
        {
            if (id != events.EventID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventsExists(events.EventID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.VenueID = new SelectList(_context.Venues, "VenueID", "VenueName", events.VenueID);
            return View(events);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var events = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (events == null)
                return NotFound();

            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var events = await _context.Events.FindAsync(id);
            if (events == null)
                return NotFound();

            try
            {
                _context.Events.Remove(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FK_Bookings_EventI") == true)
                {
                    TempData["ErrorMessage"] = "You can't delete this event because it has existing bookings.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred while trying to delete the event.";
                }

                return RedirectToAction(nameof(Index));
            }
        }

        private bool EventsExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }
    }
}
