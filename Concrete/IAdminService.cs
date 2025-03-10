namespace SimoshStoreAPI;

public interface IAdminService
{
    Task<AdminDashboardViewModel> GetDashboard();
}
