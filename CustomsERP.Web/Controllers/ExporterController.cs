using Microsoft.AspNetCore.Mvc;
using CustomsERP.Core;
using CustomsERP.Data.Context;


namespace CustomsERP.Web.Controllers;

public class ExporterController : Controller
{
    private readonly CustomsErpContext _dbContext;
    
    //Constructor
    public ExporterController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var exporters = _dbContext.Exporters.ToList();
        return View(exporters);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Exporter exporter)
    {
        if (!ModelState.IsValid)
        {
            return View(exporter);
        }
        _dbContext.Add(exporter);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int Id)
    {
        var exporter = _dbContext.Exporters.FirstOrDefault(e => e.Id == Id);
        if (exporter == null) return NotFound();
        return View(exporter);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Exporter exporter)
    {
        if (!ModelState.IsValid)
        {
            return View(exporter);
        }
        if (Id != exporter.Id) return BadRequest();
        _dbContext.Update(exporter);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int Id)
    {
        var exporter = _dbContext.Exporters.FirstOrDefault(e => e.Id == Id);
        if (exporter == null) return NotFound();
        return View(exporter);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int Id)
    {
        var exporter = _dbContext.Exporters.FirstOrDefault(e => e.Id == Id);
        if (exporter == null) return NotFound();
        _dbContext.Remove(exporter);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}