﻿namespace API.Dtos;

public class UserDto
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Bio { get; set; }
    public string AccessToken { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public string OrderId { get; set; }
    public List<string> Roles { get; set; } = [];
}
