using System;
using System.ComponentModel.DataAnnotations;

namespace PrediksiMonteCarlo.Models
{
	public class Motor
	{
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nama Motor")]
        public string NamaMotor { get; set; }
        [Required]
        [Display(Name = "Nama Merek")]
        public string Merk { get; set; }
        [Required]
        [Range(1,50000000)]
        public int Harga { get; set; }
    }
}

