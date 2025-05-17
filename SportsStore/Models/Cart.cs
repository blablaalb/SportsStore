namespace SportsStore.Models;

public class Cart
{
    // The original implementation from the book used private field and Json.NET for serialization/deserialization. We use System.Text.Json which doesn't support private properties hence we are using a public property.
    public List<CartLine> LineCollection { get; set; } = new();

    public virtual void AddItem(Product product, int quantity)
    {
        CartLine line = LineCollection
            .Where(p => p.Product.ProductID == product.ProductID)
            .FirstOrDefault();
        if (line == null)
        {
            LineCollection.Add(new CartLine
            {
                Product = product,
                Quantity = quantity
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }

    public virtual void RemoveLine(Product product) =>
        LineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);

    public virtual decimal ComputeTotalValue() => LineCollection.Sum(e => e.Product.Price * e.Quantity);

    public virtual void Clear() => LineCollection.Clear();

    public virtual IEnumerable<CartLine> Lines => LineCollection;
}

public class CartLine
{
    public int CartLineID { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}