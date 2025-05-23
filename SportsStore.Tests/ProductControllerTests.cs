using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class ProductControllerTests
{
    [Fact]
    public void Can_Paginate()
    {
        // Arrange
        Mock<IProductRepository> mock = new();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() { ProductID = 1, Name = "P1" },
            new() { ProductID = 2, Name = "P2" },
            new() { ProductID = 3, Name = "P3" },
            new() { ProductID = 4, Name = "P4" },
            new() { ProductID = 5, Name = "P5" }
        });
        ProductController controller = new(mock.Object)
        {
            PageSize = 3
        };

        // Act
        ProductsListViewModel result = controller.List(category: null, page: 2).ViewData.Model as ProductsListViewModel;

        Product[] prodArray = result.Products.ToArray();
        Assert.True(prodArray.Length == 2);
        Assert.Equal("P4", prodArray[0].Name);
        Assert.Equal("P5", prodArray[1].Name);
    }

    [Fact]
    public void Can_Send_Pagination_View_Model() 
    { 
        // Arrange
        Mock<IProductRepository> mock = new();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() { ProductID = 1, Name = "P1" },
            new() { ProductID = 2, Name = "P2" },
            new() { ProductID = 3, Name = "P3" },
            new() { ProductID = 4, Name = "P4" },
            new() { ProductID = 5, Name = "P5" }
        });

        // Arrange
        ProductController controller = new(mock.Object)
        {
            PageSize = 3
        };

        // Act
        ProductsListViewModel result = controller.List(category: null, page: 2).ViewData.Model as ProductsListViewModel;

        // Assert
        PagingInfo pageInfo = result.PagingInfo;
        Assert.Equal(2, pageInfo.CurrentPage);
        Assert.Equal(3, pageInfo.ItemsPerPage);
        Assert.Equal(5, pageInfo.TotalItems);
        Assert.Equal(2, pageInfo.TotalPages);
    }

    [Fact]
    public void Can_Filter_Products()
    {
        // Arrange
        Mock<IProductRepository> mock = new();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() { ProductID = 1, Name = "P1", Category = "Cat1" },
            new() { ProductID = 2, Name = "P2", Category = "Cat2" },
            new() { ProductID = 3, Name = "P3", Category = "Cat1" },
            new() { ProductID = 4, Name = "P4", Category = "Cat2" },
            new() { ProductID = 5, Name = "P5", Category = "Cat3" }
        });
        // Arrange
        ProductController controller = new(mock.Object)
        {
            PageSize = 3
        };
        // Act
        Product[] result = (controller.List(category: "Cat2", page: 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

        // Assert
        Assert.Equal(2, result.Length);
        Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
        Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
    }

    [Fact]
    public void Generate_Category_Specific_Product_Count()
    {
        // Arrange
        Mock<IProductRepository> mock = new();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new Product{ ProductID = 1, Name = "P1", Category = "Cat1" },
            new Product{ ProductID = 2, Name = "P2", Category = "Cat2" },
            new Product{ ProductID = 3, Name = "P3", Category = "Cat1" },
            new Product{ ProductID = 4, Name = "P4", Category = "Cat2" },
            new Product{ ProductID = 5, Name = "P5", Category = "Cat3" }
        });

        ProductController target = new(mock.Object)
        {
            PageSize = 3
        };

        Func<ViewResult, ProductsListViewModel> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

        // Act
        int? res1 = GetModel(target.List("Cat1"))?.PagingInfo.TotalItems;
        int? res2 = GetModel(target.List("Cat2"))?.PagingInfo.TotalItems;
        int? res3 = GetModel(target.List("Cat3"))?.PagingInfo.TotalItems;
        int? resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;

        // Assert
        Assert.Equal(2, res1);
        Assert.Equal(2, res2);
        Assert.Equal(1, res3);
        Assert.Equal(5, resAll);
    }
}