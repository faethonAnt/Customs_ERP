using CustomsERP.Core;

namespace CustomsERP.Web.ViewModels;

public class DocumentIndexViewModel
{
    public Shipment Shipment { get; set; } = new();

    public List<Document> Documents { get; set; } = new();
}