using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrediksiMonteCarlo.Models;

namespace PrediksiMonteCarlo.ViewModels
{
	public class PenjualanVM
	{
        public Penjualan Penjualan { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> MotorList { get; set; }
    }
}

