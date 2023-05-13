﻿using ITTasks.Models.DTOS.Users;

namespace ITTasks.Services.Users
{
    public interface IUserService
    {
        public Task<UserDto> CreateUserAsync(CreateUserDto user);
        public Task<List<UserDto>> GetAllUsersAsync();
        public Task<List<UserDto>> GetAllActiveUsersAsync();
        public Task<UserDto> GetUserByIdAsync(Guid id);
    }
}
