using System;
namespace PrediksiMonteCarlo.Models
{
	public class HasilAkhir
	{
        public string Bulan { get; set; }
        public string Tahun { get; set; }
        public int RandomNumber { get; set; }
        public int Interval { get; set; }
        public double Hasil  { get; set; }


        public List<HasilAkhir> listHasil { get; set; }
    }
}

