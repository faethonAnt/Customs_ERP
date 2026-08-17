using CustomsERP.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CustomsERP.Web.ViewModels;


public class ShipmentFilterViewModel
{
    public List<Shipment> Shipments { get; set; } = new();
    
    public int? ExporterId { get; set; }
    public int? ProductId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime?  DateTo { get; set; }

    public List<SelectListItem> Exporters { get; set; } = new();
    public List<SelectListItem> Products { get; set; } = new();
      
}