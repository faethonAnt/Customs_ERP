using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Document
{
    public int Id { get; set; }
    public int ShipmentId { get; set; }

	[Required]
	[RegularExpression(@"^[A-Z0-9]{4}.*$")]
    public string Code { get; set; }

	[Required]
    public string Name { get; set; }

    public string FilePath {get; set;}
    public DateTime CreatedOn { get; set; }
}