using Microsoft.EntityFrameworkCore;
using CustomsERP.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CustomsERP.Data.Context;

public class CustomsErpContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Exporter> Exporters { get; set; }
    public DbSet<Port> Ports { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariety> ProductVarieties { get; set; }
    public DbSet<Receiver> Receivers { get; set; }
    public DbSet<ShippingCompany> ShippingCompanies { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    

    public CustomsErpContext(DbContextOptions<CustomsErpContext> options) : base(options)
    {
    }
    
    //rules for making things like eori/Wh/port/product etc. codes correct
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Exporter>().HasIndex(e => e.Eori).IsUnique();
        modelBuilder.Entity<Port>().HasIndex(p => p.PortCode).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(p => p.HsCode).IsUnique();
        modelBuilder.Entity<Receiver>().HasIndex(r => r.Eori).IsUnique();
        modelBuilder.Entity<Warehouse>().HasIndex(w => w.WarehouseCode).IsUnique();
    }
}