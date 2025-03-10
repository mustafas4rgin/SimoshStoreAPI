using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStoreAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        
        [HttpGet("/api/my-profile/{id}")]
        public async Task<IActionResult> MyProfileAsync(int id)
        {
            var profile = await _profileService.GetProfileAsync(id);
            if(profile is null)
            {
                return NotFound();
            }
            return Ok(profile);
        }
        [HttpPut("api/update/address/{id}")]
        public async Task<IActionResult> UpdateAddressAsync(EditAddressViewModel model, int id)
        {
            var result = await _profileService.UpdateAddressAsync(id, model);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
