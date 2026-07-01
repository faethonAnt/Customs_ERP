using CustomsERP.Data.Context;
using CustomsERP.Core;
using CustomsERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		//this is done so we can view the exporters... by name and not by ids only 
        var shipments = _dbContext.Shipments
		.Include(s => s.Exporter)
		.Include(s => s.Receiver)
		.Include(s => s.ShippingCompany)
		.Include(s => s.Port)
		.Include(s => s.Warehouse)	
		.ToList();
        return View(shipments);
    }

    [HttpGet]
    public IActionResult Create()
    {
        //object init
        var shipmentForm = new ShipmentFormViewModel();
        
        //make the list of every type 
        shipmentForm.Exporters = _dbContext.Exporters
            .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name })
            .ToList();

        shipmentForm.Receivers = _dbContext.Receivers
            .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
            .ToList();

        shipmentForm.ShippingCompanies = _dbContext.ShippingCompanies
            .Select(sc => new SelectListItem { Value = sc.Id.ToString(), Text = sc.Name })
            .ToList();

        shipmentForm.Ports = _dbContext.Ports
            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.PortCode })
            .ToList();

        shipmentForm.Warehouses = _dbContext.Warehouses
            .Select(w => new SelectListItem { Value = w.Id.ToString(), Text = w.WarehouseCode })
            .ToList();

        return View(shipmentForm);
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
		
        var shipmentForm = new ShipmentFormViewModel();
		shipmentForm.Shipment = _dbContext.Shipments.FirstOrDefault(s => s.Id == id);
        if (shipmentForm.Shipment == null) return NotFound();

		//make the list of every type 
        shipmentForm.Exporters = _dbContext.Exporters
            .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name })
            .ToList();

        shipmentForm.Receivers = _dbContext.Receivers
            .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
            .ToList();

        shipmentForm.ShippingCompanies = _dbContext.ShippingCompanies
            .Select(sc => new SelectListItem { Value = sc.Id.ToString(), Text = sc.Name })
            .ToList();

        shipmentForm.Ports = _dbContext.Ports
            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.PortCode })
            .ToList();

        shipmentForm.Warehouses = _dbContext.Warehouses
            .Select(w => new SelectListItem { Value = w.Id.ToString(), Text = w.WarehouseCode })
            .ToList();

		
        return View(shipmentForm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, ShipmentFormViewModel shipmentForm)
    {
        if (shipmentForm.Shipment.Id != id) return BadRequest();
        _dbContext.Update(shipmentForm.Shipment);
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