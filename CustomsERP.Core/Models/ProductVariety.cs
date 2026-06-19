namespace CustomsERP.Core;

public class ProductVariety
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ShipmentId { get; set; }

	public int TaxCode { get; set; }
	public decimal TaxRate { get; set; }

    public decimal Value { get; set; }
    public int Quantity { get; set; }
	
}