using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrediksiMonteCarlo.Models;

namespace PrediksiMonteCarlo.ViewModels
{
	public class PrediksiVM
	{
        public Prediksi Prediksi { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> MotorList { get; set; }
        [ValidateNever]
        public IEnumerable<Penjualan> PenjualanList { get; set; }
        [ValidateNever]
        public IEnumerable<Interval> IntervalList { get; set; }
        [ValidateNever]
        public IEnumerable<AngkaAcak> AngkaAcakList { get; set; }
        [ValidateNever]
        public IEnumerable<HasilAkhir> HasilList { get; set; }
    }
}

