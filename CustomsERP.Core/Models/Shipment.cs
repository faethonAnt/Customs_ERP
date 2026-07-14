using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Shipment
{
    public int Id { get; set; }

	[Required]
    [RegularExpression(@"^\d{2}[A-Z]{4}\d{12}$")]
	public string MRN { get; set; }
    
    //Exporter reference
    public int ExporterId { get; set; }
    public Exporter Exporter { get; set; }
    
    //Receiver reference
    public int ReceiverId { get; set; }
    public Receiver Receiver { get; set; }
    
    //Shipping Company reference
    public int ShippingCompanyId { get; set; }
    public ShippingCompany ShippingCompany { get; set; }
    
    
    public int PortId { get; set; }
    public Port Port { get; set; }
    
    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    
    public List<ProductVariety> ProductVarieties { get; set; } = new();
    public List<Document> Documents { get; set; }= new();
    public bool Dv1Exists {get; set;}
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}