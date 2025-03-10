using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class ResultService : IResultService
{
    private readonly IDataRepository _dataRepository;


    public ResultService(IDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }
    public int GetUserCount()
    {
        return _dataRepository.GetAll<UserEntity>().Count();
    }
    public int GetProductCount()
    {
        var products = _dataRepository.GetAll<ProductEntity>();
        return products.Count();
    }
    public int GetCategoryCount()
    {
        var categories = _dataRepository.GetAll<CategoryEntity>();
        return categories.Count();
    }
    public int GetOrderCount()
    {
        var ordersCount = _dataRepository.GetAll<OrderEntity>().Count();
        if (ordersCount == 0)
        {
            return 0;
        }
        return ordersCount;
    }
    public int GetProductCommentCount()
    {
        var commentsCount = _dataRepository.GetAll<ProductCommentEntity>().Count();
        if (commentsCount == 0)
        {
            return 0;
        }
        return commentsCount;
    }
    public int GetBlogCommentCount()
    {
        var commentsCount = _dataRepository.GetAll<BlogCommentEntity>().Count();
        if (commentsCount == 0)
        {
            return 0;
        }
        return commentsCount;
    }
}