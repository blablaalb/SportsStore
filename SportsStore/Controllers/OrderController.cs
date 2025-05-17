using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController(IOrderRepository orderRepository, Cart cart) : Controller
{
    public ViewResult List() => View(orderRepository.Orders.Where(o => !o.Shipped));

    [HttpGet]
    public IActionResult MarkShipped(int orderID)
    {
        Order order = orderRepository.Orders.FirstOrDefault(o => o.OrderID == orderID);
        if ( order != null)
        {
            order.Shipped = true;
            orderRepository.SaveOrder(order);
        }
        return RedirectToAction(nameof(List));
    }

    public ViewResult Checkout() => View(new Order());

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (cart.Lines.Count() == 0)
        {
            ModelState.AddModelError("", "Sorry, your cart is empty!");
        }
        if (ModelState.IsValid)
        {
            order.Lines = cart.Lines.ToArray();
            orderRepository.SaveOrder(order);
            return RedirectToAction(nameof(Completed));
        }
        else
        {
            return View(order);
        }
    }

    public ViewResult Completed()
    {
        cart.Clear();
        return View();
    }
}