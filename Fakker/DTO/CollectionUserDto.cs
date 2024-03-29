﻿namespace Fakker.DTO;
public class CollectionUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public string Email { get; set; } = null!;
    public string CreatedAt { get; set; } = null!;
}