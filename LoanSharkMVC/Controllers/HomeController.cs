using LoanSharkMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSharkMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoanShark()
        {
            LoanSharkModel loanModel = new();

            loanModel.PrincipalAmount = 0.00;
            loanModel.TotalMonths = 0;
            loanModel.Rate = 0.00;
            loanModel.TotalInterest = 0.00;
            loanModel.TotalCost = 0;
            loanModel.MonthlyPayment = 0.00;

            return View(loanModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoanShark(LoanSharkModel loanModel)
        {

            double monthlyPayments = loanModel.PrincipalAmount * (loanModel.Rate / 1200.0) / (1.0 - Math.Pow(1.0 + loanModel.Rate / 1200.0, -1 * loanModel.TotalMonths));

            loanModel.MonthlyPayment = Math.Round(monthlyPayments,2);

            double totalInterest = 0.0;
            double previousBalance = loanModel.PrincipalAmount;

             List<double[]> results = new List<double[]>();

            for(int i = 0; i < loanModel.TotalMonths; i++)
            {
                double[] row = { 0.0,0.0,0.0,0.0,0.0,0.0};

                double interestPayment = previousBalance * loanModel.Rate / 1200.0;
                double principalPayment = monthlyPayments - interestPayment;

                totalInterest += interestPayment;
                previousBalance -= principalPayment;

                row[0] = i;
                row[1] = Math.Round(monthlyPayments,2);
                row[2] = Math.Round(principalPayment,2);
                row[3] = Math.Round(interestPayment,2);
                row[4] = Math.Round(totalInterest,2);
                row[5] = Math.Round(previousBalance,2);

                results.Add(row);
            }

            loanModel.TotalInterest = Math.Round(totalInterest,2);
            loanModel.TotalCost = Math.Round(loanModel.PrincipalAmount+totalInterest,2);
            loanModel.Results = results;    
            
            return View(loanModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
