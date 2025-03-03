using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class ContactFormService : IContactFormService
{
    private readonly IDataRepository _Repository;
    public ContactFormService(IDataRepository repository)
    {
        _Repository = repository;
    }
    public async Task<IServiceResult> DeleteContactFormAsync(int id)
    {
        var contactForm = await _Repository.GetByIdAsync<ContactFormEntity>(id);
        if (contactForm is null)
        {
            return new ServiceResult(false, "Contact form not found");
        }
        await _Repository.DeleteAsync<ContactFormEntity>(id);
        return new ServiceResult(true, "Contact form deleted successfully");
    }
    public async Task<IEnumerable<ContactFormEntity>> GetContactFormsAsync()
    {
        var contactForms = await _Repository.GetAll<ContactFormEntity>().ToListAsync();
        if (contactForms == null)
        {
            throw new Exception("No contact forms found");
        }
        return contactForms;
    }
}
