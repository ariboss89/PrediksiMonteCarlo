using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrediksiMonteCarlo.Models;

namespace PrediksiMonteCarlo.ViewModels
{
	public class MotorVM
	{
        public Motor Motor { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> MerkList { get; set; }
    }
}

