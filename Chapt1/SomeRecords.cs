namespace functional_pattern_practice_in_csharap;

public record Address(string Country);
public record Product(string Name, decimal Price, bool IsFood);

public record Order(Product Product, int Quantity)
{
    public decimal NetPrice => Product.Price * Quantity;
}

