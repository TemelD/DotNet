using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models;

public class CustomerDto
{
    public string Identifiant { get; init; } = default!;
    public string Firstname { get; init; } = default!;
    public string Lastname { get; init; } = default!;
    public string? MailAdress { get; init; }  = default!;
}