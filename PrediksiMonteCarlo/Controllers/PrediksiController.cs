using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PrediksiMonteCarlo.Data;
using PrediksiMonteCarlo.Models;
using PrediksiMonteCarlo.SD;
using PrediksiMonteCarlo.ViewModels;
using Syncfusion.JavaScript.Shared.Serializer;

namespace PrediksiMonteCarlo.Controllers
{
    public class PrediksiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PrediksiController(ApplicationDbContext db)
        {
            _db = db;
        }
       
        public IActionResult Index()
        {
            PrediksiVM pnjVM = new PrediksiVM()
            {
                Prediksi = new Prediksi(),

                MotorList = _db.Motors.Select(i => new SelectListItem
                {
                    Text = i.NamaMotor,
                    Value = i.Id.ToString()
                })
            };

            return View(pnjVM);

        }

        [HttpGet]
        public IActionResult Index(int? Id, string tglAwal, string tglAkhir)
        {
            if (Id == null)
            {
                PrediksiVM VMx = new PrediksiVM()
                {
                    Prediksi = new Prediksi(),

                    MotorList = _db.Motors.Select(i => new SelectListItem
                    {
                        Text = i.NamaMotor,
                        Value = i.Id.ToString()
                    })

                };

                return View(VMx);
            }

            DateTime awal = Convert.ToDateTime(tglAwal);
            DateTime akhir = Convert.ToDateTime(tglAkhir);

            if (tglAwal == "" || tglAkhir == "")
            {
                return NotFound();
            }

            var checkNama = _db.Motors.Where(x => x.Id == Id).FirstOrDefault();

            var checkData = _db.Penjualans.Where(x => x.NamaMotor == checkNama.NamaMotor).ToList();

            var data = checkData.Where(x => x.Tanggal >= awal && x.Tanggal <= akhir).ToList();

            PrediksiVM VM = new PrediksiVM()
            {
                Prediksi = new Prediksi(),

                MotorList = _db.Motors.Select(i => new SelectListItem
                {
                    Text = i.NamaMotor,
                    Value = i.Id.ToString()
                }),

                PenjualanList = data
            };

            if (VM.PenjualanList == null)
            {
                return View();
            }

            //return View(VM);

            return Json(new { data = VM.PenjualanList });

        }

        [HttpPost]
        public IActionResult Index(PrediksiVM VM)
        {
            
            DateTime awal = Convert.ToDateTime(VM.Prediksi.TanggalAwal);
            DateTime akhir = Convert.ToDateTime(VM.Prediksi.TanggalAkhir);

            if (awal.ToString() == "01/01/01 00.00.00" || akhir.ToString() == "01/01/01 00.00.00" || VM.Prediksi.NamaMotor == null)
            {
                PrediksiVM pnjVM = new PrediksiVM()
                {
                    Prediksi = new Prediksi(),

                    MotorList = _db.Motors.Select(i => new SelectListItem
                    {
                        Text = i.NamaMotor,
                        Value = i.Id.ToString()
                    })
                };

                return View(pnjVM);
            }

            string id = VM.Prediksi.NamaMotor;

            var searchMotor = _db.Motors.Where(x => x.Id == Convert.ToInt16(id)).ToList();

            var data = _db.Penjualans.Where(x => x.Tanggal >= awal && x.Tanggal <= akhir && x.NamaMotor == searchMotor[0].NamaMotor).ToList();

            if (data.Count != 0)
            {
                //rekap data dulu
                var ListOfMonths = data.GroupBy(x => x.Bulan)
                                      .Select(g => g.First())
                                      .ToList();

                List<Penjualan> listRekap = new List<Penjualan>();

                foreach (var month in ListOfMonths)
                {
                    string bulan = month.Bulan;

                    var dataRekap = data.Where(x => x.Bulan == bulan).ToList();

                    int penjualanTotal = 0;

                    for (int x = 0; x < dataRekap.Count; x++)
                    {
                        penjualanTotal += dataRekap[x].Jumlah;
                    }

                    Penjualan pnj = new Penjualan();

                    pnj.NamaMotor = dataRekap[0].NamaMotor;
                    pnj.Merk = dataRekap[0].Merk;
                    pnj.Harga = dataRekap[0].Harga;
                    pnj.Jumlah = penjualanTotal;
                    pnj.Bulan = dataRekap[0].Bulan;
                    pnj.Tahun = dataRekap[0].Tahun;

                    listRekap.Add(pnj);
                }

                //thap 2
                var count = listRekap.Count;

                Interval intr = new Interval();

                List<Interval> listInterval = new List<Interval>();

                int total = 0;

                for (int i = 0; i < count; i++)
                {
                    total += listRekap[i].Jumlah;

                }

                for (int a = 0; a < count; a++)
                {
                    intr = new Interval();

                    int set = 100 / count;

                    intr.Id = listRekap[a].Id;
                    intr.NamaMotor = listRekap[a].NamaMotor;
                    intr.Bulan = listRekap[a].Bulan;
                    intr.Tahun = listRekap[a].Tahun;
                    intr.Jumlah = listRekap[a].Jumlah;

                    double prob = Convert.ToDouble(intr.Jumlah) / Convert.ToDouble(total);

                    intr.Probabilitas = prob;

                    listInterval.Add(intr);

                }

                double kumulatif = listInterval[0].Probabilitas;

                listInterval[0].Kumulatif = kumulatif;

                for (int b = 1; b < listInterval.Count; b++)
                {

                    kumulatif += listInterval[b].Probabilitas;

                    listInterval[b].Kumulatif = kumulatif;

                }

                for (int b = 0; b < listInterval.Count; b++)
                {
                    double divided = 100 / listInterval.Count;

                    double max = divided * (b + 1);

                    listInterval[b].Id = b + 1;

                    listInterval[0].Min = 1;

                    listInterval[0].Max = divided;

                    if (b == 1)
                    {
                        listInterval[1].Min = divided + 1;
                        listInterval[b].Max = max;
                    }

                    if (b > 1)
                    {

                        double minManise = listInterval[b - 1].Max + 1;

                        listInterval[b].Min = minManise;

                        listInterval[b].Max = max;

                    }

                    if (b == listInterval.Count - 1)
                    {
                        listInterval[listInterval.Count - 1].Max = 100;
                    }


                }


                //tahap3
                var ListOfYears = data.GroupBy(x => x.Tahun)
                                      .Select(g => g.First())
                                      .ToList();

                List<string> listBulan = new List<string>();

                listBulan.Add("January");
                listBulan.Add("February");
                listBulan.Add("March");
                listBulan.Add("April");
                listBulan.Add("May");
                listBulan.Add("June");
                listBulan.Add("July");
                listBulan.Add("August");
                listBulan.Add("September");
                listBulan.Add("October");
                listBulan.Add("November");
                listBulan.Add("December");

                int countList = listInterval.Count;

                string dataBulan = listInterval[countList - 1].Bulan;

                Dictionary<string, int> tahunBulan = new Dictionary<string, int>();

                List<string> listBulanAcak = new List<string>();

                int tahunFix = 0;

                for (int i = 0; i < listBulan.Count; i++)
                {
                    string bulan = listBulan[i].ToString();
                    int countRkp = listRekap.Count();

                    if (bulan.Equals(dataBulan))
                    {
                        int gg = i + 1;

                        if (i == 11)
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                listBulanAcak.Add(listBulan[j].ToString());

                                int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                                tahunFix = tahun + 1;

                                tahunBulan.Add(listBulan[j].ToString(), tahunFix);
                            }
                        }

                        if (i == 10)
                        {
                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            listBulanAcak.Add("December");
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);

                            for (int j = 0; j < 6; j++)
                            {
                                listBulanAcak.Add(listBulan[j].ToString());
                                tahunFix = tahun + 1;
                                tahunBulan.Add(listBulan[j].ToString(), tahunFix);

                            }
                        }

                        if (i == 9)
                        {
                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            listBulanAcak.Add("November");
                            listBulanAcak.Add("December");
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);

                            for (int j = 0; j < 5; j++)
                            {
                                listBulanAcak.Add(listBulan[j].ToString());
                                tahunFix = tahun + 1;
                                tahunBulan.Add(listBulan[j].ToString(), tahunFix);
                            }
                        }

                        if (i == 8)
                        {
                            listBulanAcak.Add("October");
                            listBulanAcak.Add("November");
                            listBulanAcak.Add("December");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);

                            for (int j = 0; j < 4; j++)
                            {
                                listBulanAcak.Add(listBulan[j].ToString());
                                tahunFix = tahun + 1;
                                tahunBulan.Add(listBulan[j].ToString(), tahunFix);
                            }
                        }

                        if (i == 7)
                        {
                            listBulanAcak.Add("September");
                            listBulanAcak.Add("October");
                            listBulanAcak.Add("November");
                            listBulanAcak.Add("December");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[3].ToString(), tahun);

                            for (int j = 0; j < 3; j++)
                            {
                                listBulanAcak.Add(listBulan[j].ToString());
                                tahunFix = tahun + 1;
                                tahunBulan.Add(listBulan[j].ToString(), tahunFix);
                            }
                        }

                        if (i == 6)
                        {
                            listBulanAcak.Add("Agustus");
                            listBulanAcak.Add("September");
                            listBulanAcak.Add("October");
                            listBulanAcak.Add("November");
                            listBulanAcak.Add("December");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[3].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[4].ToString(), tahun);

                            for (int j = 0; j < 2; j++)
                            {
                                listBulanAcak.Add(listBulan[j].ToString());
                                tahunFix = tahun + 1;
                                tahunBulan.Add(listBulan[j].ToString(), tahunFix);
                            }
                        }

                        if (i == 5)
                        {
                            listBulanAcak.Add("July");
                            listBulanAcak.Add("Agustus");
                            listBulanAcak.Add("September");
                            listBulanAcak.Add("October");
                            listBulanAcak.Add("November");
                            listBulanAcak.Add("December");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[3].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[4].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[5].ToString(), tahun);

                        }

                        if (i == 4)
                        {
                            listBulanAcak.Add("June");
                            listBulanAcak.Add("July");
                            listBulanAcak.Add("Agustus");
                            listBulanAcak.Add("September");
                            listBulanAcak.Add("October");
                            listBulanAcak.Add("November");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[3].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[4].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[5].ToString(), tahun);

                        }

                        if (i == 3)
                        {
                            listBulanAcak.Add("May");
                            listBulanAcak.Add("June");
                            listBulanAcak.Add("July");
                            listBulanAcak.Add("Agustus");
                            listBulanAcak.Add("September");
                            listBulanAcak.Add("October");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[3].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[4].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[5].ToString(), tahun);
                        }

                        if (i == 2)
                        {
                            listBulanAcak.Add("March");
                            listBulanAcak.Add("April");
                            listBulanAcak.Add("May");
                            listBulanAcak.Add("June");
                            listBulanAcak.Add("July");
                            listBulanAcak.Add("Agustus");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[3].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[4].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[5].ToString(), tahun);

                        }

                        if (i == 1)
                        {
                            listBulanAcak.Add("February");
                            listBulanAcak.Add("March");
                            listBulanAcak.Add("April");
                            listBulanAcak.Add("May");
                            listBulanAcak.Add("June");
                            listBulanAcak.Add("July");

                            int tahun = Convert.ToInt16(listRekap[countRkp - 1].Tahun);
                            tahunBulan.Add(listBulanAcak[0].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[1].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[2].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[3].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[4].ToString(), tahun);
                            tahunBulan.Add(listBulanAcak[5].ToString(), tahun);
                        }
                    }
                }

                List<AngkaAcak> listAcak = new List<AngkaAcak>();

                foreach (var item in tahunBulan)
                {
                    AngkaAcak ac = new AngkaAcak();
                    Random rand = new Random();
                    int number = rand.Next(1, 100);

                    ac.Bulan = item.Key;
                    ac.Tahun = item.Value.ToString();
                    ac.RandomNumber = number;
                    listAcak.Add(ac);
                }

                List<HasilAkhir> listHasil = new List<HasilAkhir>();

                foreach (var m in listAcak)
                {
                    HasilAkhir ha = new HasilAkhir();

                    int acak = m.RandomNumber;
                    ha.Tahun = m.Tahun;
                    ha.Bulan = m.Bulan;
                    ha.RandomNumber = acak;


                    var cekInterval = listInterval.Where(x => x.Min <= acak && x.Max >= acak).ToList();

                    int interval = cekInterval[0].Id;

                    ha.Interval = interval;
                    ha.Hasil = cekInterval[0].Jumlah;

                    listHasil.Add(ha);

                }

                StaticDetails_Login.HasilList = listHasil;

                VM = new PrediksiVM()
                {
                    Prediksi = new Prediksi(),

                    MotorList = _db.Motors.Select(i => new SelectListItem
                    {
                        Text = i.NamaMotor,
                        Value = i.Id.ToString()
                    }),

                    PenjualanList = listRekap,

                    IntervalList = listInterval,

                    AngkaAcakList = listAcak,

                    HasilList = listHasil
                };
            }

            if (VM.PenjualanList == null)
            {
                PrediksiVM pnjVM = new PrediksiVM()
                {
                    Prediksi = new Prediksi(),

                    MotorList = _db.Motors.Select(i => new SelectListItem
                    {
                        Text = i.NamaMotor,
                        Value = i.Id.ToString()
                    })
                };

                return View(pnjVM);
            }

            return View(VM);
        }

        [HttpGet]
        public IActionResult Print()
        {
            List<HasilAkhir> listAkhir = new List<HasilAkhir>();

            listAkhir = StaticDetails_Login.HasilList;
            return View(listAkhir);
        }

        //[HttpPost]
        //public IActionResult Print(string data)
        //{
        //    //if(dataJson == null)
        //    //{
        //    //    return Json("Hehehe");
        //    //}

        //    //HasilAkhir[] user = JsonConvert.DeserializeObject<HasilAkhir[]>(dataJson);

        //    return Json("User Details are updated");


        //}

        //public IActionResult Print(PrediksiVM prdVM)
        //{
        //    PrediksiVM VM = new PrediksiVM()
        //    {
        //        HasilList = prdVM.HasilList
        //    };

        //    return View(VM);
        //}

        //[HttpPost]
        //public JsonResult Print(string json)
        //{
        //    var serializer = new JavaScriptSerializer();
        //    dynamic jsondata = serializer.Deserialize(json, typeof(object));
        //    List<string> myfieldName = new List<string>();
        //    //Access your array now
        //    foreach (var item in jsondata)
        //    {
        //        myfieldName.Add(item["myfieldName"]);
        //    }

        //    //Do something with the list here
        //    return Json("Success");
        //}


        #region API CALLS

        //[HttpGet]
        //public IActionResult GetDataPenjualan(string tglAwal, string tglAkhir)
        //{

        //    DateTime awal = Convert.ToDateTime(tglAwal);
        //    DateTime akhir = Convert.ToDateTime(tglAkhir);

        //    if (tglAwal == "" || tglAkhir == "")
        //    {
        //        return NotFound();
        //    }

        //    var data = _db.Penjualans.Where(x => x.Tanggal >= awal && x.Tanggal <= akhir).ToList();

        //    PrediksiVM VM = new PrediksiVM()
        //    {
        //        Prediksi = new Prediksi(),

        //        MotorList = _db.Motors.Select(i => new SelectListItem
        //        {
        //            Text = i.NamaMotor,
        //            Value = i.Id.ToString()
        //        }),

        //        PenjualanList = data
        //    };

        //    if (VM.PenjualanList == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(new { data = VM.PenjualanList });

        //}

        #endregion

    }


}