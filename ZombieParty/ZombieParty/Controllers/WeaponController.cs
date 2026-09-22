using Microsoft.AspNetCore.Mvc;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.ViewModels;

namespace ZombieParty.Controllers
{
    public class WeaponController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public WeaponController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        public IActionResult Index()
        {
            List<Weapon> weapons = _baseDonnees.Weapons.ToList();
            return View(weapons);
        }

        public IActionResult Upsert(int? id)
        {
            // Insert if null (ou 0)
            if (id == null || id == 0) return View(new Weapon());

            Weapon? recherche = _baseDonnees.Weapons.Where(w => w.WeaponId == id).SingleOrDefault();

            // 404 if doesn't exist
            if (recherche == null) return NotFound();

            // Update if exists
            return View(recherche);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Upsert(Weapon weapon, int? id)
        {
            if (ModelState.IsValid)
            {
                // Ajouter à la BD (id null, donc 0)
                if (id == null || id == 0)
                {
                    _baseDonnees.Weapons.Add(weapon);
                    TempData["Success"] = $"{weapon.Name} weapon added";
                    _baseDonnees.SaveChanges();

                    return this.RedirectToAction("Index");
                }

                

                // sinon, tout est bon, update
                // recherche = weapon;
                _baseDonnees.Weapons.Update(weapon);

                TempData["Success"] = $"{weapon.Name} weapon updated";

                _baseDonnees.SaveChanges();
                return this.RedirectToAction("Index");

            }

            return this.View(weapon);
        }
    }
}
