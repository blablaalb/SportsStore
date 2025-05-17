﻿
namespace SportsStore.Models;

public class EFProductRepository(ApplicationDbContext context) : IProductRepository
{
    public IEnumerable<Product> Products => context.Products;

    public void SaveProduct(Product product)
    {
        if (product.ProductID == 0)
        {
            context.Products.Add(product);
        }
        else
        {
            Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
            if (dbEntry != null)
            {
                dbEntry.Name = product.Name;
                dbEntry.Description = product.Description;
                dbEntry.Price = product.Price;
                dbEntry.Category = product.Category;
            }
        }
        context.SaveChanges();
    }

    public Product DeleteProduct(int productID)
    {
        Product dbEntity = context.Products.FirstOrDefault(p => p.ProductID == productID);
        if (dbEntity != null)
        {
            context.Products.Remove(dbEntity);
            context.SaveChanges();
        }
        return dbEntity;
    }
}
