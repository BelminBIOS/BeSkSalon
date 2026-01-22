using BeSkSalon.Data;
using BeSkSalon.Models;
using BeSkSalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BeSkSalon.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Dashboard
        public async Task<IActionResult> Index()
        {
            var termini = await _context.Termini
                .Include(t => t.Frizer)
                .Include(t => t.Usluga)
                .Include(t => t.User)
                .OrderBy(t => t.Datum)
                .ThenBy(t => t.VrijemeOd)
                .ToListAsync();
            return View(termini);
        }
        #region Frizeri
        // GET: Admin/Frizeri
        public async Task<IActionResult> Frizeri()
        {
            var frizeri = await _context.Frizeri.ToListAsync();
            return View(frizeri);
        }
        // GET: Admin/KreirajFrizera
        public IActionResult KreirajFrizera()
        {
            return View();
        }
        // POST: Admin/KreirajFrizera
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KreirajFrizera(FrizerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var frizer = new Frizer
                {
                    Ime = model.Ime,
                    Tip = model.Tip
                };
                _context.Frizeri.Add(frizer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Frizer uspje�no kreiran!";
                return RedirectToAction(nameof(Frizeri));
            }
            return View(model);
        }
        // GET: Admin/UrediFrizera/5
        public async Task<IActionResult> UrediFrizera(int? id)
        {
            if (id == null) return NotFound();
            var frizer = await _context.Frizeri.FindAsync(id);
            if (frizer == null) return NotFound();
            var model = new FrizerViewModel
            {
                Id = frizer.Id,
                Ime = frizer.Ime,
                Tip = frizer.Tip
            };
            return View(model);
        }
        // POST: Admin/UrediFrizera/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediFrizera(int id, FrizerViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var frizer = await _context.Frizeri.FindAsync(id);
                if (frizer == null) return NotFound();
                frizer.Ime = model.Ime;
                frizer.Tip = model.Tip;
                _context.Update(frizer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Frizer uspje�no a�uriran!";
                return RedirectToAction(nameof(Frizeri));
            }
            return View(model);
        }
        // POST: Admin/ObrisiFrizera/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObrisiFrizera(int id)
        {
            var frizer = await _context.Frizeri.FindAsync(id);
            if (frizer != null)
            {
                _context.Frizeri.Remove(frizer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Frizer uspje�no obrisan!";
            }
            return RedirectToAction(nameof(Frizeri));
        }
        #endregion
        #region Usluge
        // GET: Admin/Usluge
        public async Task<IActionResult> Usluge()
        {
            var usluge = await _context.Usluge
                .Include(u => u.Frizer)
                .ToListAsync();
            return View(usluge);
        }
        // GET: Admin/KreirajUslugu
        public async Task<IActionResult> KreirajUslugu()
        {
            var model = new UslugaViewModel
            {
                Frizeri = await _context.Frizeri.ToListAsync()
            };
            return View(model);
        }
        // POST: Admin/KreirajUslugu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KreirajUslugu(UslugaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usluga = new Usluga
                {
                    Naziv = model.Naziv,
                    Trajanje = model.Trajanje,
                    Cijena = model.Cijena,
                    FrizerId = model.FrizerId
                };
                _context.Usluge.Add(usluga);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Usluga uspje�no kreirana!";
                return RedirectToAction(nameof(Usluge));
            }
            model.Frizeri = await _context.Frizeri.ToListAsync();
            return View(model);
        }
        // GET: Admin/UrediUslugu/5
        public async Task<IActionResult> UrediUslugu(int? id)
        {
            if (id == null) return NotFound();
            var usluga = await _context.Usluge.FindAsync(id);
            if (usluga == null) return NotFound();
            var model = new UslugaViewModel
            {
                Id = usluga.Id,
                Naziv = usluga.Naziv,
                Trajanje = usluga.Trajanje,
                Cijena = usluga.Cijena,
                FrizerId = usluga.FrizerId,
                Frizeri = await _context.Frizeri.ToListAsync()
            };
            return View(model);
        }
        // POST: Admin/UrediUslugu/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediUslugu(int id, UslugaViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var usluga = await _context.Usluge.FindAsync(id);
                if (usluga == null) return NotFound();
                usluga.Naziv = model.Naziv;
                usluga.Trajanje = model.Trajanje;
                usluga.Cijena = model.Cijena;
                usluga.FrizerId = model.FrizerId;
                _context.Update(usluga);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Usluga uspje�no a�urirana!";
                return RedirectToAction(nameof(Usluge));
            }
            model.Frizeri = await _context.Frizeri.ToListAsync();
            return View(model);
        }
        // POST: Admin/ObrisiUslugu/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObrisiUslugu(int id)
        {
            var usluga = await _context.Usluge.FindAsync(id);
            if (usluga != null)
            {
                _context.Usluge.Remove(usluga);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Usluga uspje�no obrisana!";
            }
            return RedirectToAction(nameof(Usluge));
        }
        #endregion
        #region Termini
        // POST: Admin/OtkaziTermin/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OtkaziTermin(int id)
        {
            var termin = await _context.Termini.FindAsync(id);
            if (termin != null)
            {
                termin.Status = "Otkazan";
                _context.Update(termin);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Termin uspje�no otkazan!";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}