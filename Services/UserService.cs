using System.Security.Claims;
using App.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataRepository _dataRepository;

    public UserService(IHttpContextAccessor httpContextAccessor, IDataRepository dataRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataRepository = dataRepository;
    }


    public async Task<UserEntity> GetUserByIdAsync(int id)
    {
        var user = await _dataRepository.GetByIdAsync<UserEntity>(id);
        if (user is null)
        {
            throw new Exception("User not found");
        }
        return user;
    }
    public async Task<IEnumerable<UserEntity>> GetUsersAsync()
    {
        var users = await _dataRepository.GetAll<UserEntity>().ToListAsync();
        if (users == null)
        {
            throw new Exception("No users found");
        }
        return users;
    }
    public async Task<bool> ValidateUserAsync(string email, string password)
    {
        var users = await GetUsersAsync();
        var user = users.Where(x => x.Email == email && HashingHelper.VerifyPasswordHash(password,x.PasswordHash,x.PasswordSalt)).FirstOrDefault();
        if (user == null)
        {
            return false;
        }
        return true;
    }
    public async Task<IServiceResult> ValidateUserRoleAsync(UserEntity user)
    {
        var role = await _dataRepository.GetByIdAsync<RoleEntity>(user.RoleId);
        if (role is null)
        {
            return new ServiceResult(false, "Role not found");
        }
        user.Role = role;
        return new ServiceResult(true, "Role found");
    }
    public async Task<UserEntity> GetUserByEmailAsync(string email)
    {
        var user = await _dataRepository.GetAll<UserEntity>().Where(x => x.Email == email).FirstOrDefaultAsync();
        if (user is null)
        {
            throw new Exception("User not found");
        }
        return user;
    }
    public async Task<IServiceResult> UpdateUserAsync(UserDTO dto, int id)
    {
        var user = await GetUserByIdAsync(id);
        user.Address = dto.Address;
        user.Email = dto.Email;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Phone = dto.Phone;
        user.RoleId = dto.RoleId;
        HashingHelper.CreatePasswordHash(dto.Password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _dataRepository.UpdateAsync(user);
        return new ServiceResult(true, "User updated successfully");

    }
    public async Task<IServiceResult> DeleteUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        await _dataRepository.DeleteAsync<UserEntity>(id);
        return new ServiceResult(true, "User deleted successfully");
    }
    public async Task<IServiceResult> CreateUserAsync(UserDTO dto)
    {
        var user = MappingHelper.MappingUserEntity(dto);
        await _dataRepository.AddAsync(user);
        return new ServiceResult(true, "User created successfully");
    }
}
