using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSharkMVC.Models
{
    public class LoanSharkModel
    {
         
        public double PrincipalAmount { get; set; }
        public int TotalMonths { get; set; }
        public double Rate { get; set; }
        public List<double[]> Results { get; set; } = new List<double[]>();

        public double TotalInterest { get; set; }

        public double TotalCost { get; set; }

        public double MonthlyPayment { get; set; }
    }
}
