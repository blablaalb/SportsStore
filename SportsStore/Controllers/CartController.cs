﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;
public class CartController(IProductRepository productRepository, Cart cart) : Controller
{
    public RedirectToActionResult AddToCart(int productId, string returnUrl)
    {
        Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
        if (product != null)
        {
            cart.AddItem(product, 1);
            SaveCart(cart);
        }
        return RedirectToAction("Index", new { returnUrl });
    }

    public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
    {
        Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productId);
        if (product != null)
        {
            cart.RemoveLine(product);
            SaveCart(cart);
        }
        return RedirectToAction("Index", new { returnUrl });
    }

    public ViewResult Index(string returnUrl)
    {
        return View(new CartIndexViewModel
        {
            Cart = cart,
            ReturnUrl = returnUrl
        });
    }

    //private Cart GetCart()
    //{
    //    Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
    //    return cart;
    //}

    private void SaveCart(Cart cart)
    {
        HttpContext.Session.SetJson("Cart", cart);
    }
}
