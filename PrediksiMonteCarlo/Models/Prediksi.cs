using System;
using System.ComponentModel.DataAnnotations;

namespace PrediksiMonteCarlo.Models
{
	public class Prediksi
	{
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Id Prediksi")]
        public string IdPrediksi { get; set; }
        [Required]
        [Display(Name = "Nama Motor")]
        public string NamaMotor { get; set; }
        [Required]
        [Display(Name = "Tanggal Awal")]
        public DateTime? TanggalAwal { get; set; }
        [Required]
        [Display(Name = "Tanggal Akhir")]
        public DateTime? TanggalAkhir { get; set; }
    }
}

