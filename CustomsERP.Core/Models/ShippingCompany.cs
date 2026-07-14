using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class ShippingCompany
{
    public int Id { get; set; }
	
	[Required]
    public string Name { get; set; }
    
	[Required]
    public string Address { get; set; }
	
	[Required]
    public string City { get; set; }

	[RegularExpression(@"^[A-Z]{2}\d{2}[A-Z0-9]{11,30}$")]
    public string BankDetails { get; set; }
}