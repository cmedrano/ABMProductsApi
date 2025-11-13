namespace Products.Domain.Entities
{
    public class Product
    {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        
    }
}
