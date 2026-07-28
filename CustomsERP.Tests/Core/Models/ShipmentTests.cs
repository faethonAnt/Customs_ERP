
using System.ComponentModel.DataAnnotations;
using CustomsERP.Core;

namespace CustomsERP.Tests.Core.Models;
public class ShipmentTests
{
    [Fact]
    public void MRN_RejectsValueThatIsTooShort()
    {
        var shipment = new Shipment { MRN = "TOO SHORT" };
        var context = new ValidationContext(shipment) { MemberName = nameof(Shipment.MRN) };
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateProperty(shipment.MRN, context, results);

        Assert.False(isValid);
    }
    
    [Fact]
    public void MRN_AcceptsValidValue()
    {
        var shipment = new Shipment { MRN = "26GRIM400100846767" };
        var context = new ValidationContext(shipment) { MemberName = nameof(Shipment.MRN) };
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateProperty(shipment.MRN, context, results);

        Assert.True(isValid);
    }
}