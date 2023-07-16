using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrediksiMonteCarlo.Data;
using PrediksiMonteCarlo.Models;

namespace PrediksiMonteCarlo.Controllers
{
    public class MerkController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MerkController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var objCategoryList = _db.Merks.ToList();

            return View(objCategoryList);
        }

        public IActionResult Upsert(int? Id)
        {
            Merk merk = new Merk();
            if (Id == null)
            {
                return View(merk);
            }

            //this is for edit
            var mrk = _db.Merks.Where(x => x.Id == Id).ToList();

            merk.Id = mrk[0].Id;
            merk.Nama = mrk[0].Nama;
            
            if (merk == null)
            {
                return NotFound();
            }

            return View(merk);
        }

        [HttpPost]
        public IActionResult Upsert(Merk mrk)
        {
            if (ModelState.IsValid)
            {
                if (mrk.Id == 0)
                {
                    _db.Merks.Add(mrk);
                    _db.SaveChanges();
                }
                else
                {
                    _db.Merks.Update(mrk);

                }

                _db.SaveChanges();

                TempData["success"] = "Merek berhasil di tambahkan";

                return RedirectToAction("Index");
            }

            TempData["error"] = "Merk gagal di tambahkan";

            return View(mrk);

        }

        public IActionResult Delete(int? Id)
        {
            Merk? data = new Merk();

            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            data = _db.Merks.Find(Id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Merk ctg = _db.Merks.Find(id);

            if (ctg == null)
            {
                return NotFound();
            }

            _db.Merks.Remove(ctg);
            _db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}