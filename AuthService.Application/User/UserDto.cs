﻿
namespace AuthService.Application.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
