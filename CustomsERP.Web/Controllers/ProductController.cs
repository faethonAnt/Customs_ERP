using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace CustomsERP.Web.Controllers;

public class ProductController : Controller
{
    private readonly CustomsErpContext _dbContext;
    
    public ProductController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
        
    }

    public IActionResult Index()
    {
        var products = _dbContext.Products.ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        _dbContext.Add(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int Id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == Id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Product product)
    {
        if (Id != product.Id) return BadRequest();
        _dbContext.Update(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Delete(int Id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == Id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int Id, Product product)
    {
        if (Id != product.Id) return BadRequest();
        _dbContext.Remove(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
}