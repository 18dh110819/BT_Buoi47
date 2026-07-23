public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public string Image { get; set; } = "";
    public ProductCategoryDto productCategoryDto { get; set; } = new ProductCategoryDto();
    public ProductShopDto productShopDto { get; set; } = new ProductShopDto();
}

public class ProductCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class ProductShopDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}