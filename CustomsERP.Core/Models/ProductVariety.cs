using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class ProductVariety
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ShipmentId { get; set; }
	
	[Required]
	[Range(100,420)]
	public int TaxCode { get; set; }
    
    [Required]
	public decimal TaxRate { get; set; }
	
	[Required]
    public decimal Value { get; set; }
    
	[Required]
    public int Quantity { get; set; }
	
}