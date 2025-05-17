using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class CartSummaryViewComponent(Cart cart) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View(cart);
    }
}
