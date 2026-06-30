using CustomsERP.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CustomsERP.Web.ViewModels;

public class ShipmentFormViewModel
{
    public Shipment Shipment { get; set; } = new();
    public List<SelectListItem> Exporters { get; set; } = new();
    public List<SelectListItem> Receivers { get; set; } = new();
    public List<SelectListItem> ShippingCompanies { get; set; } = new();
    public List<SelectListItem> Ports { get; set; } = new();
    public List<SelectListItem> Warehouses { get; set; } = new();
    
}