using CustomsERP.Data.Context;
using CustomsERP.Core;
using Microsoft.AspNetCore.Mvc;

namespace CustomsERP.Web.Controllers;


public class ShipmentController : Controller
{
    private readonly CustomsErpContext _dbContext;

    public ShipmentController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var shipments = _dbContext.Shipments.ToList();
        return View(shipments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Shipment shipment)
    {
        _dbContext.Add(shipment);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var shipment = _dbContext.Shipments.FirstOrDefault(s => s.Id == id);
        if (shipment == null) return NotFound();
        return View(shipment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, Shipment shipment)
    {
        if (shipment.Id != id) return BadRequest();
        _dbContext.Update(shipment);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var shipment = _dbContext.Shipments.FirstOrDefault(s => s.Id == id);
        if (shipment == null) return NotFound();
        return View(shipment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var shipment = _dbContext.Shipments.FirstOrDefault(s => s.Id == id);
        if (shipment == null) return NotFound();
        _dbContext.Remove(shipment);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}