namespace CustomsERP.Core;

public class Document
{
    public int Id { get; set; }
    public int ShipmentId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string FilePath {get; set;}
    public DateTime CreatedOn { get; set; }
}