using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Receiver
{
    public int Id { get; set; }
	[Required]
    [StringLength(16)]
    [RegularExpression(@"^GR\d{14}$")]
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
	[RegularExpression(@"^[A-Z]{2}$")]
    public string CountryCode { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}