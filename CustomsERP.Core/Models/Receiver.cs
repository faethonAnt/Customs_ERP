namespace CustomsERP.Core;

public class Receiver
{
    public int Id { get; set; }
    public string Eori { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}