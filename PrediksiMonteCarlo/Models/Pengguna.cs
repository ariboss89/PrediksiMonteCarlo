using System;
using System.ComponentModel.DataAnnotations;

namespace PrediksiMonteCarlo.Models
{
	public class Pengguna
	{
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		public string Roles { get; set; }
	}
}

