using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests;

public class AdminControllerTests
{
    [Fact]
    public void Index_Contains_All_Products()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        mock.Setup(m => m.Products).Returns(new Product[] {
            new Product { ProductID = 1, Name = "P1" },
            new Product { ProductID = 2, Name = "P2" },
            new Product { ProductID = 3, Name = "P3" }
        });
        var target = new AdminController(mock.Object);
        // Act
        Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();
        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal("P1", result[0].Name);
        Assert.Equal("P2", result[1].Name);
        Assert.Equal("P3", result[2].Name);
    }

    [Fact]
    public void Can_Edit_Product()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        mock.Setup(m => m.Products).Returns(new Product[] {
            new Product { ProductID = 1, Name = "P1" },
            new Product { ProductID = 2, Name = "P2" },
            new Product { ProductID = 3, Name = "P3" }
        });
        var target = new AdminController(mock.Object);
        // Act
        Product p1 = GetViewModel<Product>(target.Edit(1));
        Product p2 = GetViewModel<Product>(target.Edit(2));
        Product p3 = GetViewModel<Product>(target.Edit(3));
        // Assert
        Assert.Equal(1, p1.ProductID);
        Assert.Equal(2, p2.ProductID);
        Assert.Equal(3, p3.ProductID);
    }

    [Fact]
    public void Cannot_Edit_Nonexistent_Product()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        mock.Setup(m => m.Products).Returns(new Product[] {
            new Product { ProductID = 1, Name = "P1" },
            new Product { ProductID = 2, Name = "P2" },
            new Product { ProductID = 3, Name = "P3" }
        });
        var target = new AdminController(mock.Object);
        // Act
        Product result = GetViewModel<Product>(target.Edit(4));
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Can_Save_Valid_Changes()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        var tempData = new Mock<ITempDataDictionary>();
        var target = new AdminController(mock.Object) { TempData = tempData.Object };
        var product = new Product { Name = "Test" };

        // Act
        IActionResult result = target.Edit(product);

        // Assert
        // Check that the repository was called
        mock.Verify(m => m.SaveProduct(product));
        // Check the result type is a redirection
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
    }

    [Fact]
    public void Cannot_Save_Invalid_Changes()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        var target = new AdminController(mock.Object);
        var product = new Product { Name = "Test" };
        target.ModelState.AddModelError("error", "error");
        // Act
        IActionResult result = target.Edit(product);
        // Assert
        // Check that the repository was not called
        mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
        // Check the result type is a view result
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Can_Delete_Valid_Products()
    {
        // Arrange
        Product product = new Product { ProductID = 2, Name = "Test" };
        var mock = new Mock<IProductRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new Product { ProductID = 1, Name = "P1" },
            product,
            new Product { ProductID = 3, Name = "P3" }
        });
        var target = new AdminController(mock.Object);
        
        // Act
        target.Delete(product.ProductID);

        // Assert
        // Ensure that the repository delete method was called with the correct product ID
        mock.Verify(m => m.DeleteProduct(product.ProductID));
    }

    private T GetViewModel<T>(IActionResult result) where T : class
    {
        return (result as ViewResult)?.ViewData.Model as T;
    }
}
