using App.Data.Entities;
using SimoshStore;

namespace SimoshStoreAPI;

public interface IContactFormService
{
    Task<IServiceResult> DeleteContactFormAsync(int id);
    Task<IEnumerable<ContactFormEntity>> GetContactFormsAsync();
}
