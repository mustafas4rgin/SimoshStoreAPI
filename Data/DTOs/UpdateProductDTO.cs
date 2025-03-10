using App.Data.Entities;

namespace SimoshStoreAPI;

public class UpdateProductDTO
{
    public List<CategoryEntity> Categories { get; set; }
    public List<DiscountEntity> Discounts { get; set; }
    public ProductEntity Product { get; set; }
}
