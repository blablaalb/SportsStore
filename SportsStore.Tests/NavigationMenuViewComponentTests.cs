using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests;

public class NavigationMenuViewComponentTests
{
    [Fact]
    public void Can_Select_Categories()
    {
        // Arrange
        var mock = new Mock<IProductRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() { ProductID = 1, Name = "P1", Category = "Apples" },
            new() { ProductID = 2, Name = "P2", Category = "Apples" },
            new() { ProductID = 3, Name = "P3", Category = "Plums" },
            new() { ProductID = 4, Name = "P4", Category = "Oranges" }
        });
        var target = new NavigationMenuViewComponent(mock.Object);

        // Act
        var result = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

        // Assert
        Assert.Equal(new[] { "Apples", "Oranges", "Plums" }, result);
    }

    [Fact]
    public void Indicates_Selected_Category()
    {
        // Arrange
        string categoryToSelect = "Apples";
        Mock<IProductRepository> mock = new Mock<IProductRepository>();
        mock.Setup(m => m.Products).Returns(new Product[]
        {
            new() { ProductID = 1, Name = "P1", Category = "Apples" },
            new() { ProductID = 4, Name = "P4", Category = "Oranges" } 
        });
        NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
        target.ViewComponentContext = new ViewComponentContext 
        {
            ViewContext = new ViewContext
            {
                RouteData = new RouteData()
            }
        };
        target.RouteData.Values["category"] = categoryToSelect;

        // Act
        string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

        // Assert
        Assert.Equal(categoryToSelect, result);
    }
}
