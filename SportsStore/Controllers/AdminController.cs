using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers;

public class AdminController(IProductRepository productRepository) : Controller
{
    public ViewResult Index()
    {
        return View(productRepository.Products);
    }

    public ViewResult Edit(int productId) => View(productRepository.Products.FirstOrDefault(p => p.ProductID == productId));

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            productRepository.SaveProduct(product);
            TempData["message"] = $"{product.Name} has been saved.";
            return RedirectToAction("Index");
        }
        else
        {
            // there is thing wrong with the data values
            return View(product);
        }
    }

    public ViewResult Create() => View("Edit", new Product());

    [HttpPost]
    public IActionResult Delete(int productID)
    {
        Product deletedProduct = productRepository.DeleteProduct(productID);
        if (deletedProduct != null)
        {
            TempData["message"] = $"{deletedProduct.Name} was deleted";
        }
        return RedirectToAction("Index");
    }
}
