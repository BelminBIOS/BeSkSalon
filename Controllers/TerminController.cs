using BeSkSalon.Data;
using BeSkSalon.Models;
using BeSkSalon.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BeSkSalon.Controllers
{
    [Authorize]
    public class TerminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TerminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var frizeri = await _context.Frizeri.ToListAsync();
            var model = new ZakaziTerminViewModel
            {
                Frizeri = frizeri,
                Datum = DateTime.Today
            };
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsluge(int frizerId)
        {
            var usluge = await _context.Usluge
                .Where(u => u.FrizerId == frizerId)
                .Select(u => new
                {
                    u.Id,
                    u.Naziv,
                    u.Trajanje,
                    u.Cijena
                })
                .ToListAsync();
            return Json(usluge);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetSlobodniTermini(int frizerId, DateTime datum, int uslugaId)
        {
            
            if (!RadnoVrijemeValidator.IsRadniDan(datum))
            {
                return Json(new
                {
                    success = false,
                    message = "Salon ne radi nedjeljom. Molimo odaberite drugi dan."
                });
            }
            var usluga = await _context.Usluge.FindAsync(uslugaId);
            if (usluga == null)
            {
                return Json(new { success = false, message = "Usluga nije prona?ena" });
            }
            
            var radnoVrijemeOd = RadnoVrijemeValidator.RadnoVrijemeOd;
            var radnoVrijemeDo = RadnoVrijemeValidator.RadnoVrijemeDo;
            var trajanje = usluga.Trajanje;
            
            var zauzetiTermini = await _context.Termini
                .Where(t => t.FrizerId == frizerId &&
                           t.Datum.Date == datum.Date &&
                           t.Status == "Zakazan")
                .OrderBy(t => t.VrijemeOd)
                .ToListAsync();
            
            var slobodniTermini = new List<TimeSpan>();
            var trenutnoVrijeme = radnoVrijemeOd;
            
            while (trenutnoVrijeme.Add(TimeSpan.FromMinutes(trajanje)) <= radnoVrijemeDo)
            {
                var krajTermina = trenutnoVrijeme.Add(TimeSpan.FromMinutes(trajanje));
                
                if (krajTermina > radnoVrijemeDo)
                {
                    break;
                }
                var isSlobodan = true;
                
                foreach (var zauzetiTermin in zauzetiTermini)
                {
                    if ((trenutnoVrijeme >= zauzetiTermin.VrijemeOd && trenutnoVrijeme < zauzetiTermin.VrijemeDo) ||
                        (krajTermina > zauzetiTermin.VrijemeOd && krajTermina <= zauzetiTermin.VrijemeDo) ||
                        (trenutnoVrijeme <= zauzetiTermin.VrijemeOd && krajTermina >= zauzetiTermin.VrijemeDo))
                    {
                        isSlobodan = false;
                        break;
                    }
                }
                if (isSlobodan)
                {
                    slobodniTermini.Add(trenutnoVrijeme);
                }
                trenutnoVrijeme = trenutnoVrijeme.Add(TimeSpan.FromMinutes(30));
            }
            
            if (slobodniTermini.Count == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Izvinjavamo se, nemamo slobodan termin u odabranom periodu."
                });
            }
            return Json(new
            {
                success = true,
                termini = slobodniTermini.Select(t => new
                {
                    value = t.ToString(@"hh\:mm"),
                    text = t.ToString(@"hh\:mm")
                })
            });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zakazi(ZakaziTerminViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var usluga = await _context.Usluge.FindAsync(model.UslugaId);
            if (usluga == null)
            {
                TempData["Error"] = "Odabrana usluga nije pronađena.";
                return RedirectToAction(nameof(Index));
            }
            var vrijemeDo = model.VrijemeOd.Add(TimeSpan.FromMinutes(usluga.Trajanje));
            
            var (isValid, errorMessage) = RadnoVrijemeValidator.ValidateRadnoVrijeme(
                model.Datum, 
                model.VrijemeOd, 
                vrijemeDo
            );
            if (!isValid)
            {
                TempData["Error"] = errorMessage;
                return RedirectToAction(nameof(Index));
            }
            
            var postojiTermin = await _context.Termini
                .AnyAsync(t => t.FrizerId == model.FrizerId &&
                              t.Datum.Date == model.Datum.Date &&
                              t.Status == "Zakazan" &&
                              ((model.VrijemeOd >= t.VrijemeOd && model.VrijemeOd < t.VrijemeDo) ||
                               (vrijemeDo > t.VrijemeOd && vrijemeDo <= t.VrijemeDo) ||
                               (model.VrijemeOd <= t.VrijemeOd && vrijemeDo >= t.VrijemeDo)));
            if (postojiTermin)
            {
                TempData["Error"] = "Izvinjavamo se, odabrani termin je već zauzet.";
                return RedirectToAction(nameof(Index));
            }
            var termin = new Termin
            {
                FrizerId = model.FrizerId,
                UslugaId = model.UslugaId,
                Datum = model.Datum,
                VrijemeOd = model.VrijemeOd,
                VrijemeDo = vrijemeDo,
                UserId = user.Id,
                Status = "Zakazan"
            };
            _context.Termini.Add(termin);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Termin uspješno zakazan!";
            return RedirectToAction(nameof(MojiTermini));
        }
        
        public async Task<IActionResult> MojiTermini()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var termini = await _context.Termini
                .Include(t => t.Frizer)
                .Include(t => t.Usluga)
                .Where(t => t.UserId == user.Id)
                .OrderByDescending(t => t.Datum)
                .ThenByDescending(t => t.VrijemeOd)
                .Select(t => new TerminDetaljiViewModel
                {
                    Id = t.Id,
                    Datum = t.Datum,
                    VrijemeOd = t.VrijemeOd,
                    VrijemeDo = t.VrijemeDo,
                    Status = t.Status,
                    FrizerIme = t.Frizer.Ime,
                    UslugaNaziv = t.Usluga.Naziv,
                    UslugaCijena = t.Usluga.Cijena,
                    UslugaTrajanje = t.Usluga.Trajanje
                })
                .ToListAsync();
            var model = new MojiTerminiViewModel
            {
                Termini = termini
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Otkazi(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var termin = await _context.Termini
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            if (termin == null)
            {
                TempData["Error"] = "Termin nije pronađen.";
                return RedirectToAction(nameof(MojiTermini));
            }
            if (termin.Status == "Otkazan")
            {
                TempData["Error"] = "Termin je već otkazan.";
                return RedirectToAction(nameof(MojiTermini));
            }
            termin.Status = "Otkazan";
            _context.Update(termin);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Termin uspješno otkazan.";
            return RedirectToAction(nameof(MojiTermini));
        }
    }
}
