using System;
using System.ComponentModel.DataAnnotations;

namespace PrediksiMonteCarlo.Models
{
	public class Merk
	{
		[Key]
		public int Id { get; set; }
		[Required]
        [Display(Name = "Nama Merek")]
        public string Nama { get; set; }
	}
}

