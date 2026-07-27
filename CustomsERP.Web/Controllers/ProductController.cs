using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace CustomsERP.Web.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly CustomsErpContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    
    public ProductController(CustomsErpContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;

    }

    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        var products = User.IsInRole("Admin")
            ? _dbContext.Products.ToList() : _dbContext.Products.Where(p => p.UserId == userId).ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        ModelState.Remove(nameof(Product.UserId));
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        product.UserId = _userManager.GetUserId(User);
        _dbContext.Add(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Update(int Id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == Id);
        if (product == null) return NotFound();
        if (product.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, Product product)
    {
        var existingProduct = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == Id);
        if (existingProduct == null) return NotFound();

        if (existingProduct.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
        
        ModelState.Remove(nameof(Product.UserId));
        if (!ModelState.IsValid)
        {
            return View(product);
        }
        if (Id != product.Id) return BadRequest();
        
        product.UserId = existingProduct.UserId;
        _dbContext.Update(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Delete(int Id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == Id);
        if (product == null) return NotFound();
        if (product.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int Id)
    {
        var product = _dbContext.Products.FirstOrDefault(p => p.Id == Id);
        if (product == null) return NotFound();
        if (product.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
        _dbContext.Remove(product);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
}