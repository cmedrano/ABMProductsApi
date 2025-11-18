namespace Products.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        // Propiedad para la relación muchos-a-muchos
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();


    }
}
