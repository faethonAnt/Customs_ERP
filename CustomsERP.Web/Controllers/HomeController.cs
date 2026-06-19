using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CustomsERP.Web.Models;
using CustomsERP.Data.Context;

namespace CustomsERP.Web.Controllers;

public class HomeController : Controller
{
    private readonly CustomsErpContext _dbContext; // db connection
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    //db Connection
    public HomeController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }

    //list of EXPORTERS
    public IActionResult Exporters()
    {
        var exporters = _dbContext.Exporters.ToList();
        return View(exporters);
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
