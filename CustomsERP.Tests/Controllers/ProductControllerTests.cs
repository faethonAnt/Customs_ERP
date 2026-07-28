using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using CustomsERP.Core;
using CustomsERP.Data.Context;
using CustomsERP.Web.Controllers;

namespace CustomsERP.Tests.Controllers;

public class ProductControllerTests
{
    [Fact]
    public void Index_OnlyShowsCurrentUsersProducts()
    {
        var options = new DbContextOptionsBuilder<CustomsErpContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new CustomsErpContext(options);

        context.Products.Add(new Product { HsCode = "1111111111", Name = "Mine", UserId = "user1" });
        context.Products.Add(new Product { HsCode = "2222222222", Name = "NotMine", UserId = "user2" });
        context.SaveChanges();

        var storeMock = new Mock<IUserStore<IdentityUser>>();
        var userManagerMock = new Mock<UserManager<IdentityUser>>(storeMock.Object, null, null, null, null, null, null, null, null);
        userManagerMock.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user1");

        var controller = new ProductController(context, userManagerMock.Object);
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
        };

        var result = controller.Index() as ViewResult;
        var model = result!.Model as List<Product>;

        Assert.Single(model!);
        Assert.Equal("Mine", model![0].Name);
    }
}
