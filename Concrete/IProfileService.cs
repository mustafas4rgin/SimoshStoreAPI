using SimoshStore;

namespace SimoshStoreAPI;

public interface IProfileService
{
    Task<ProfileViewModel> GetProfileAsync(int userId);
    Task<IServiceResult> UpdateAddressAsync(int id,EditAddressViewModel profile);
}
