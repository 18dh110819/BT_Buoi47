using System.Net;
using Cybersoft_store.Helper;
using Cybersoft_store.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

public interface ICategoryService
{
    Task<ResponseType<List<CategoryDto>>> GetAllCategoriesAsync(string? keyword, int page = 1, int pageSize = 10);
    Task<ResponseType<string>> CreateCategoryAsync(CategoryCreateDTO model);
}


public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<ResponseType<string>> CreateCategoryAsync(CategoryCreateDTO model)
    {
        try
        {
            var exist = await _categoryRepository.GetFirstOrDefaultAsync(x => x.Name == model.Name && x.ShopId == model.ShopId);
            if (exist != null)
            {
                return new ResponseType<string>
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    Message = CategoryResponseMessage.CreateCategoryAlreadyExists,
                    DataResponse = CategoryResponseMessage.CreateCategoryAlreadyExists,
                    Timestamp = DateTime.UtcNow
                };
            }

            var alias = HelperFunction.StringToSlug(model.Name);
            //207
            var category = new Category
            {
                Name = model.Name,
                Alias = alias,
                ShopId = model.ShopId,
                Deleted = false
            };

            await _categoryRepository.AddAsync(category);

            return new ResponseType<string>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = CategoryResponseMessage.CreateCategorySuccess,
                DataResponse = CategoryResponseMessage.CreateCategorySuccess,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();

            return new ResponseType<string>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = CategoryResponseMessage.CreateCategoryFailed,
                DataResponse = CategoryResponseMessage.CreateCategoryFailed,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    public async Task<ResponseType<List<CategoryDto>>> GetAllCategoriesAsync(string? keyword, int page = 1, int pageSize = 10)
    {
        var slug = HelperFunction.StringToSlug(keyword ?? string.Empty);

        var categories = await _categoryRepository.GetWhereAsync(c => c.Deleted == false && (string.IsNullOrEmpty(slug) || c.Alias.Contains(slug)));

        var lst = await categories.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Alias = c.Alias
            }).ToListAsync();

        return new ResponseType<List<CategoryDto>>
        {
            StatusCode = 200,
            DataResponse = lst,
            Message = CategoryResponseMessage.GetAllCategorySuccess,
            Timestamp = DateTime.UtcNow
        };
    }
}