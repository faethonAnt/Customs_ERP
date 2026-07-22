using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Port
{
    public int Id { get; set; }
	
	[Required]
	[StringLength(50)]
	[RegularExpression(@"^GR\d{6}.*$", ErrorMessage = "Port Code should start with GR followed by 6 digits")]
    public string PortCode { get; set; }
    
}