using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CustomsERP.Web.Controllers;

[Authorize]
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
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Warehouse warehouse)
    {
        if (!ModelState.IsValid)
        {
            return View(warehouse);
        }
        _dbContext.Add(warehouse);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Update(int id)
    {
        var warehouse = _dbContext.Warehouses.FirstOrDefault(p => p.Id == id);
        if (warehouse == null) return NotFound();
        return View(warehouse);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Warehouse warehouse)
    {
        if (!ModelState.IsValid)
        {
            return View(warehouse);
        }
        if (Id != warehouse.Id) return BadRequest();
        _dbContext.Update(warehouse);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var warehouse = _dbContext.Warehouses.FirstOrDefault(p => p.Id == id);
        if (warehouse == null) return NotFound();
        return View(warehouse);
    }

    [Authorize(Roles = "Admin")]
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