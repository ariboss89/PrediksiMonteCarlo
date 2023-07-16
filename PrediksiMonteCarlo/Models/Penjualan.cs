using System;
using System.ComponentModel.DataAnnotations;

namespace PrediksiMonteCarlo.Models
{
	public class Penjualan
	{
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nama Motor")]
        public string NamaMotor { get; set; }
        [Display(Name = "Nama Merek")]
        public string Merk { get; set; }
        [Display(Name = "Harga Motor")]
        public int Harga { get; set; }
        [Required]
        [Display(Name = "Jumlah Penjualan")]
        [Range(1, 1000)]
        public int Jumlah { get; set; }
        [Required]
        [Display(Name ="Tanggal Penjualan")]
        public DateTime? Tanggal { get; set; }
        [Required]
        public string Bulan { get; set; }
        [Required]
        public string Tahun { get; set; }

    }
}

