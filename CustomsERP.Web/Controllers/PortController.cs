using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.AspNetCore.Mvc;

namespace CustomsERP.Web.Controllers;

public class PortController : Controller
{
    private readonly CustomsErpContext _dbContext;
    
    public PortController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var ports = _dbContext.Ports.ToList();
        return View(ports);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Port Port)
    {
        if (!ModelState.IsValid)
        {
            return View(Port);
        }
        _dbContext.Add(Port);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var port = _dbContext.Ports.FirstOrDefault(p => p.Id == id);
        if (port == null) return NotFound();
        return View(port);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Port port)
    {
        if (!ModelState.IsValid)
        {
            return View(port);
        }
        if (Id != port.Id) return BadRequest();
        _dbContext.Update(port);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var port = _dbContext.Ports.FirstOrDefault(p => p.Id == id);
        if (port == null) return NotFound();
        return View(port);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var port = _dbContext.Ports.FirstOrDefault(p => p.Id == id);
        if (port == null) return NotFound();
        _dbContext.Remove(port);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}