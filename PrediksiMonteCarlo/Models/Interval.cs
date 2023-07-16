using System;
using System.ComponentModel.DataAnnotations;

namespace PrediksiMonteCarlo.Models
{
	public class Interval
	{
        public int Id { get; set; }
        [Display(Name = "Nama Motor")]
        public string NamaMotor { get; set; }
        public string Bulan { get; set; }
        public string Tahun { get; set; }
        [Display(Name = "Jumlah Penjualan")]
        [Range(1, 1000)]
        public int Jumlah { get; set; }
        public double Probabilitas { get; set; }
        public double Kumulatif { get; set; }
        [Range(1,100)]
        public double Min { get; set; }
        [Range(1, 100)]
        public double Max { get; set; }
    }
}

