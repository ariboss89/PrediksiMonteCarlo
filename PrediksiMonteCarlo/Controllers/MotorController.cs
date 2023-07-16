using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using PrediksiMonteCarlo.Data;
using PrediksiMonteCarlo.Models;
using PrediksiMonteCarlo.ViewModels;

namespace PrediksiMonteCarlo.Controllers
{
    public class MotorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MotorController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var objCategoryList = _db.Motors.ToList();

            return View(objCategoryList);
        }

        public IActionResult Upsert(int? id)
        {
            var datas = _db.Motors.Find(id);

            MotorVM motorVM = new MotorVM()
            {
                Motor = new Motor(),

                MerkList = _db.Merks.Select(i => new SelectListItem
                {
                    Text = i.Nama,
                    Value = i.Id.ToString()
                })
            };


            if (id == null)
            {
                return View(motorVM);
            }

            var data = _db.Motors.Where(x => x.Id == id).ToList();

            motorVM.Motor.Id = data[0].Id;
            motorVM.Motor.NamaMotor = data[0].NamaMotor;
            motorVM.Motor.Merk = data[0].Merk;
            motorVM.Motor.Harga = data[0].Harga;

            if (motorVM.Motor == null)
            {
                return NotFound();
            }

            return View(motorVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Upsert(MotorVM motorVM)
        {
            if (ModelState.IsValid)
            {
                var data = _db.Merks.Where(x => x.Id == Convert.ToInt32(motorVM.Motor.Merk)).FirstOrDefault();

                if (motorVM.Motor.Id == 0)
                {
                    motorVM.Motor.Merk = data.Nama;

                    _db.Motors.Add(motorVM.Motor);
                    _db.SaveChanges();
                }
                else
                {

                    motorVM.Motor.Merk = data.Nama;
                    _db.Motors.Update(motorVM.Motor);

                }

                _db.SaveChanges();

                TempData["success"] = "Motor berhasil di tambahkan";

                return RedirectToAction("Index");
            }

            else
            {

                TempData["error"] = "Motor gagal di tambahkan";

                motorVM.MerkList = _db.Merks.Select(i => new SelectListItem
                {
                    Text = i.Nama,
                    Value = i.Id.ToString()
                });

                return View(motorVM);
            }
        }

    }
}