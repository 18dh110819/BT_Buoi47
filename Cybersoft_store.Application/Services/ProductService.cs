using Cybersoft_store.Helper;
using Microsoft.EntityFrameworkCore;

public interface IProductService
{
    Task<ResponseType<List<ProductDto>>> GetAll(string? keyword, int page = 1, int pageSize = 10);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseType<List<ProductDto>>> GetAll(string? keyword, int page = 1, int pageSize = 10)
    {
        try
        {
            var slug = HelperFunction.StringToSlug(keyword ?? string.Empty);
            var products = await _productRepository.GetWhereAsync(x => x.Deleted == false && (string.IsNullOrEmpty(keyword) || x.Alias.Contains(slug)));

            var productDtos = await products.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.DisplayPrice,
                Image = p.Image,
                productCategoryDto = new ProductCategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name ?? ""
                },
                productShopDto = new ProductShopDto
                {
                    Id = p.Shop.Id,
                    Name = p.Shop.ShopName ?? "",
                    Description = p.Shop.Description ?? ""
                }
            }).ToListAsync();

            return new ResponseType<List<ProductDto>>
            {
                StatusCode = 200,
                Message = "Products retrieved successfully",
                DataResponse = productDtos,
                Timestamp = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            // Handle error if necessary
            return new ResponseType<List<ProductDto>>
            {
                StatusCode = 500,
                Message = "An error occurred while retrieving products",
                DataResponse = null,
                Timestamp = DateTime.Now
            };
        }
    }
}