using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class ProductController(IProductRepository productRepository) : Controller
{
    public int PageSize { get; set; } = 4;

    public ViewResult List(int page = 1) => View(new ProductsListViewModel
        {
            Products = productRepository.Products
                .OrderBy(p => p.ProductID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = productRepository.Products.Count()
            }
        }
    );
}
