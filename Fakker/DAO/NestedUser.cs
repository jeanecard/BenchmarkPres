﻿namespace Fakker.DAO;
public class NestedUser
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public NestedAddress Address { get; set; } = null!;
}