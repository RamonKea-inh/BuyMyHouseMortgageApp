using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuyMyHouseMortgageApp.Controllers
{
    public class MortgageApplicationController : Controller
    {
        private readonly IMortgageApplicationRepository _repository;

        public MortgageApplicationController(IMortgageApplicationRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MortgageApplication application)
        {
            if (ModelState.IsValid)
            {
                await _repository.CreateMortgageApplicationAsync(application);
                return RedirectToAction("ThankYou");
            }
            return View(application);
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
