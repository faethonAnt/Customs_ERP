using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Port
{
    public int Id { get; set; }
	
	[Required]
	[StringLength(50)]
	[RegularExpression(@"^GR\d{6}.*$")]
    public string PortCode { get; set; }
    
}