using Microsoft.AspNetCore.Mvc;
using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.AspNetCore.Authorization;


namespace CustomsERP.Web.Controllers;

[Authorize]
public class ReceiverController : Controller
{
    private readonly CustomsErpContext _dbContext;
    
    // Constructor
    public ReceiverController(CustomsErpContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var receivers = _dbContext.Receivers.ToList();
        return View(receivers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Receiver Receiver)
    {
        if (!ModelState.IsValid)
        {
            return View(Receiver);
        }
        _dbContext.Add(Receiver);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int Id)
    {
        var receiver = _dbContext.Receivers.FirstOrDefault(r => r.Id == Id);
        if (receiver == null) return NotFound();
        return View(receiver);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Receiver receiver)
    {
        if (!ModelState.IsValid)
        {
            return View(receiver);
        }
        if (Id != receiver.Id) return BadRequest();
        _dbContext.Update(receiver);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int Id)
    {
        var receiver = _dbContext.Receivers.FirstOrDefault(r => r.Id == Id);
        if (receiver == null) return NotFound();
        return View(receiver);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int Id)
    {
        var receiver = _dbContext.Receivers.FirstOrDefault(r => r.Id == Id);
        if (receiver == null) return NotFound();
        _dbContext.Remove(receiver);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}