using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class AdminService : IAdminService
{
    private readonly IDataRepository _repository;
    private readonly IResultService _resultService;
    public AdminService(IDataRepository repository,IResultService resultService)
    {
        _repository = repository;
        _resultService = resultService;
    }
    public async Task<AdminDashboardViewModel> GetDashboard()
    {
        int totalCategories = _resultService.GetCategoryCount();
        int totalProducts = _resultService.GetProductCount();
        int totalOrders = _resultService.GetOrderCount();
        int totalUsers = _resultService.GetUserCount();

        var latestOrders = await _repository.GetAll<OrderEntity>()
                           .Include(O=> O.User)
                           .OrderByDescending(O => O.CreatedAt)
                           .ToListAsync();
        if(latestOrders is null)
        {
            latestOrders = new List<OrderEntity>();
        }
        var latestComments = await _repository.GetAll<ProductCommentEntity>()
                            .Include(O => O.Product)
                            .ToListAsync();

        
        if(latestComments is null)
        {
            latestComments = new List<ProductCommentEntity>();
        }

        var latestBlogs = await _repository.GetAll<BlogEntity>()
                            .Include(P => P.User)
                            .OrderByDescending(B => B.CreatedAt).ToListAsync();

        if(latestBlogs is null)
        {
            latestBlogs = new List<BlogEntity>();
        }
        return new AdminDashboardViewModel
        {
            LatestBlogs = latestBlogs,
            TotalCategories = totalCategories,
            TotalProducts = totalProducts,
            TotalOrders = totalOrders,
            TotalUsers = totalUsers,
            LatestOrders = latestOrders,
            LatestComments = latestComments,
        };
    }
}
