using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Receiver
{
    public int Id { get; set; }
	[Required]
    [StringLength(16)]
    [RegularExpression(@"^GR\d{14}$", ErrorMessage = "Eori Code should start with GR followed by 14 digits")]
    public string Eori { get; set; }

	[Required]
    public string Name { get; set; }

	[Required]
    public string Address { get; set; }
    
	[Required]
    public string City { get; set; }
    
	[Required]
    public string ZipCode { get; set; }
    
	[Required]
	[RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Country code must be only two capital letters")]
    public string CountryCode { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}