using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using PrediksiMonteCarlo.Data;
using PrediksiMonteCarlo.Models;
using PrediksiMonteCarlo.ViewModels;

namespace PrediksiMonteCarlo.Controllers
{
    public class PenjualanController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PenjualanController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var datas = _db.Penjualans.Find(id);

            PenjualanVM pnjVM = new PenjualanVM()
            {
                Penjualan = new Penjualan(),

                MotorList = _db.Motors.Select(i => new SelectListItem
                {
                    Text = i.NamaMotor,
                    Value = i.Id.ToString()
                })
            };


            if (id == null)
            {
                return View(pnjVM);
            }

            var data = _db.Penjualans.Where(x => x.Id == id).ToList();

            pnjVM.Penjualan.Id = data[0].Id;
            pnjVM.Penjualan.NamaMotor = data[0].NamaMotor;
            pnjVM.Penjualan.Merk = data[0].Merk;
            pnjVM.Penjualan.Harga = data[0].Harga;
            pnjVM.Penjualan.Tanggal = data[0].Tanggal;
            pnjVM.Penjualan.Bulan = data[0].Bulan;
            pnjVM.Penjualan.Tahun = data[0].Tahun;

            if (pnjVM.Penjualan == null)
            {
                return NotFound();
            }

            return View(pnjVM);
           
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Upsert(PenjualanVM pnjVM)
        {
            if (pnjVM.Penjualan.NamaMotor!="")
            {
                var data = _db.Motors.Where(x => x.Id == Convert.ToInt32(pnjVM.Penjualan.NamaMotor)).FirstOrDefault();

                if (pnjVM.Penjualan.Id == 0)
                {
                    pnjVM.Penjualan.NamaMotor = data.NamaMotor;
                    pnjVM.Penjualan.Merk = data.Merk;
                    pnjVM.Penjualan.Harga = data.Harga;
                    DateTime? tanggal = pnjVM.Penjualan.Tanggal;
                    string bulan = tanggal?.ToString("MMMM");
                    string tahun = tanggal?.ToString("yyyy");

                    pnjVM.Penjualan.Tanggal = tanggal;
                    pnjVM.Penjualan.Bulan = bulan;
                    pnjVM.Penjualan.Tahun = tahun;

                    _db.Penjualans.Add(pnjVM.Penjualan);
                    _db.SaveChanges();
                }
                else
                {
                    pnjVM.Penjualan.NamaMotor = data.NamaMotor;
                    pnjVM.Penjualan.Merk = data.Merk;
                    pnjVM.Penjualan.Harga = data.Harga;
                    DateTime? tanggal = pnjVM.Penjualan.Tanggal;
                    string bulan = tanggal?.ToString("MMMM");
                    string tahun = tanggal?.ToString("yyyy");
                    pnjVM.Penjualan.Tanggal = tanggal;
                    pnjVM.Penjualan.Bulan = bulan;
                    pnjVM.Penjualan.Tahun = tahun;
                    pnjVM.Penjualan.Merk = data.Merk;
                    _db.Penjualans.Update(pnjVM.Penjualan);

                }

                _db.SaveChanges();

                TempData["success"] = "Penjualan berhasil di tambahkan";

                return RedirectToAction("Index");
            }

            else
            {

                TempData["error"] = "Penjualan gagal di tambahkan";

                pnjVM.MotorList = _db.Merks.Select(i => new SelectListItem
                {
                    Text = i.Nama,
                    Value = i.Id.ToString()
                });

                return View(pnjVM);
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Penjualan> penjualanList = _db.Penjualans.ToList();

            return Json(new { data = penjualanList });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var objFromDb = _db.Penjualans.Find(Id);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }

            _db.Penjualans.Remove(objFromDb);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete Successful" });
        }

        [HttpGet]
        public IActionResult GetDataMotor(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            PenjualanVM VM = new PenjualanVM()
            {
                Penjualan = new Penjualan(),

                MotorList = _db.Motors.Select(i => new SelectListItem
                {
                    Text = i.NamaMotor,
                    Value = i.Id.ToString()
                })
            };

            var data = _db.Motors.Where(x => x.Id == id).FirstOrDefault();


            VM.Penjualan.NamaMotor = data.NamaMotor;
            VM.Penjualan.Merk = data.Merk;
            VM.Penjualan.Harga = data.Harga;

            //if (pnjVM.Penjualan == null)
            //{
            //    return NotFound();
            //}

            return Json(new { data = VM.Penjualan });

        }

        #endregion
    }
}