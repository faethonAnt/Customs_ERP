using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Exporter
{
    public int Id { get; set; }
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