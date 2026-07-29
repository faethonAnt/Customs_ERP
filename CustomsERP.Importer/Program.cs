using CsvHelper;
using CsvHelper.Configuration;
using CustomsERP.Core;
using CustomsERP.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var optionsBuilder = new DbContextOptionsBuilder<CustomsErpContext>();
optionsBuilder.UseNpgsql("Host=localhost;Database=customserp;Username=postgres;Password=postgres");

using var context = new CustomsErpContext(optionsBuilder.Options);

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = false
};

using var reader = new StreamReader("products.csv");
using var csv = new CsvReader(reader, config);

var seenCodes = new HashSet<string>();
var productsToAdd = new List<Product>();

var adminUser = context.Users.FirstOrDefault(u => u.Email == "faethon21@gmail.com");
if (adminUser == null)
{
    throw new InvalidOperationException("Admin user not found — log into the web app at least once first so the account exists.");
}

while (csv.Read())
{
    var rawCode = csv.GetField(0);
    var description = csv.GetField(4);

    var code = rawCode.Split(' ')[0];
    if (!seenCodes.Add(code)) continue;
    
    productsToAdd.Add(new Product{HsCode = code, Name = description, UserId = adminUser.Id});
}

context.Products.AddRange(productsToAdd);
context.SaveChanges();

Console.WriteLine($"Imported {productsToAdd.Count} products.");