using System.ComponentModel.DataAnnotations;

namespace CustomsERP.Core;

public class Warehouse
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(16)]
    [RegularExpression(@"^GR\d{14}$")]
    public string WarehouseCode { get; set; }
}