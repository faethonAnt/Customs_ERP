using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace CustomsERP.Web.Controllers;

public class WarehouseController : Controller
{
     private readonly CustomsErpContext _dbContext;
    
    public WarehouseController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var warehouse = _dbContext.Warehouses.ToList();
        return View(warehouse);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Warehouse warehouse)
    {
        _dbContext.Add(warehouse);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var warehouse = _dbContext.Warehouses.FirstOrDefault(p => p.Id == id);
        if (warehouse == null) return NotFound();
        return View(warehouse);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Warehouse warehouse)
    {
        if (Id != warehouse.Id) return BadRequest();
        _dbContext.Update(warehouse);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var warehouse = _dbContext.Warehouses.FirstOrDefault(p => p.Id == id);
        if (warehouse == null) return NotFound();
        return View(warehouse);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var warehouse = _dbContext.Warehouses.FirstOrDefault(p => p.Id == id);
        if (warehouse == null) return NotFound();
        _dbContext.Remove(warehouse);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}