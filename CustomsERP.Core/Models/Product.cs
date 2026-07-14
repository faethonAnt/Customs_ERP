using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Product
{
    public int Id { get; set; }
    
    [Required]
    [RegularExpression(@"\d{10}$")]
    public required string HsCode { get; set; }
    
    [Required]
    public required string Name { get; set; }
}
