using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace CustomsERP.Web.Controllers;

[Authorize]
public class ShippingCompanyController : Controller
{
    private readonly CustomsErpContext _dbContext;

    public ShippingCompanyController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var shippingCompany = _dbContext.ShippingCompanies.ToList();
        return View(shippingCompany);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShippingCompany shippingCompany)
    {
        if (!ModelState.IsValid)
        {
            return View(shippingCompany);
        }
        _dbContext.Add(shippingCompany);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int Id)
    {
        var shippingCompany = _dbContext.ShippingCompanies.FirstOrDefault(sc => sc.Id == Id);
        if (shippingCompany == null) return NotFound();
        return View(shippingCompany);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, ShippingCompany shippingCompany)
    {
        if (!ModelState.IsValid)
        {
            return View(shippingCompany);
        }
        if (shippingCompany.Id != Id) return BadRequest();
        _dbContext.Update(shippingCompany);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int Id)
    {
        var shippingCompany = _dbContext.ShippingCompanies.FirstOrDefault(sc => sc.Id == Id);
        if (shippingCompany == null) return NotFound();
        return View(shippingCompany);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int Id)
    {
        var shippingCompany = _dbContext.ShippingCompanies.FirstOrDefault(sc => sc.Id == Id);
        if (shippingCompany == null) return NotFound();
        _dbContext.Remove(shippingCompany);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
}