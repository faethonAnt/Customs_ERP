
using CustomsERP.Data.Context;
using CustomsERP.Web.ViewModels;
using CustomsERP.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomsERP.Web.Controllers;

public class DocumentController : Controller
{
    private readonly CustomsErpContext _dbContext;

    public DocumentController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index(int ShipmentId)
    {
        //search for shipment
       var shipment = _dbContext.Shipments.FirstOrDefault(s => s.Id == ShipmentId);
       if (shipment == null) return NotFound();
       // search for documents of said shipment
       var documents = _dbContext.Documents.Where(d => d.ShipmentId == ShipmentId).ToList();
       var viewModel = new DocumentIndexViewModel{Shipment =shipment, Documents = documents};
       return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create(int shipmentId)
    {
        //search for shipment
        var shipment = _dbContext.Shipments.FirstOrDefault(s => s.Id == shipmentId);
        if (shipment == null) return NotFound();
        return View(new Document{ShipmentId = shipmentId});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Document document)
    {
        document.CreatedOn = DateTime.UtcNow;
        _dbContext.Add(document);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new {ShipmentId = document.ShipmentId});
    }

    [HttpGet]
    public IActionResult Update(int Id)
    {
        var document = _dbContext.Documents.FirstOrDefault(d => d.Id == Id);
        if (document == null) return NotFound();
        return View(document);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Document document)
    {
        if (Id != document.Id) return BadRequest();
        _dbContext.Update(document);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new {ShipmentId = document.ShipmentId});
    }

    [HttpGet]
    public IActionResult Delete(int Id)
    {
        var document = _dbContext.Documents.FirstOrDefault(d => d.Id == Id);
        if (document == null) return NotFound();
        return View(document);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int Id)
    {
        var document = _dbContext.Documents.FirstOrDefault(d => d.Id == Id);
        if (document == null) return NotFound();
        _dbContext.Remove(document);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new{ShipmentId = document.ShipmentId});
    } 
    
}