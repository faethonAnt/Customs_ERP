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
        _dbContext.Add(exporter);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
}