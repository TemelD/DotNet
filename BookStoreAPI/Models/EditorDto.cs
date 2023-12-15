using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models;

public class EditorDto
{
    public string Firstname { get; init; } = default!;
    public string? Lastname { get; init; }  = default!;
}