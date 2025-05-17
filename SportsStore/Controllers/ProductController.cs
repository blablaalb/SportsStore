using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class ProductController(IProductRepository productRepository) : Controller
{
    public int PageSize { get; set; } = 4;

    public ViewResult List(string category, int page = 1) => View(new ProductsListViewModel
    {
        Products = productRepository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
        PagingInfo = new PagingInfo
        {
            CurrentPage = page,
            ItemsPerPage = PageSize,
            TotalItems = category == null ?
                productRepository.Products.Count() :
                productRepository.Products.Count(e => e.Category == category)
        },
        CurrentCategory = category
    });
}
