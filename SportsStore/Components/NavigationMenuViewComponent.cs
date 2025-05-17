using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class NavigationMenuViewComponent(IProductRepository productRepository) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        ViewBag.SelectedCategory = RouteData?.Values["category"];
        return View(productRepository.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
    }
}
