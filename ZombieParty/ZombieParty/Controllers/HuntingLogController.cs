using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZombieParty.Models;
using ZombieParty.Models.Data;

namespace ZombieParty.Controllers
{
    public class HuntingLogController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public HuntingLogController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }
        // GET: HuntingLogController
        public ActionResult Index()
        {
            return View(_baseDonnees.HuntingLogs.ToList());
        }

        public IActionResult Upsert(int? id)
        {
            // Insert if null (ou 0)
            if (id == null || id == 0) return View(new HuntingLog());

            HuntingLog? recherche = _baseDonnees.HuntingLogs.Where(h => h.Id == id).SingleOrDefault();

            // 404 if doesn't exist
            if (recherche == null) return NotFound();

            // Update if exists
            return View(recherche);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Upsert(HuntingLog huntingLog, int? id)
        {
            if (ModelState.IsValid)
            {
                // Ajouter à la BD (id null, donc 0)
                if (id == null || id == 0)
                {
                    _baseDonnees.HuntingLogs.Add(huntingLog);
                    TempData["Success"] = $"Log {huntingLog.Title} added";
                    _baseDonnees.SaveChanges();

                    return this.RedirectToAction("Index");
                }



                // sinon, tout est bon, update
                // recherche = weapon;
                _baseDonnees.HuntingLogs.Update(huntingLog);

                TempData["Success"] = $"Log {huntingLog.Title} updated";

                _baseDonnees.SaveChanges();
                return this.RedirectToAction("Index");

            }

            return this.View(huntingLog);
        }
    }
}
