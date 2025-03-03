using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStoreAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;
        public ContactFormController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }
        [HttpGet]
        public async Task<IActionResult> GetContactForms()
        {
            try
            {
                var contactForms = await _contactFormService.GetContactFormsAsync();
                return Ok(contactForms);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("/contactform/delete/{id}")]
        public async Task<IActionResult> DeleteContactForm(int id)
        {
            var result = await _contactFormService.DeleteContactFormAsync(id);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
