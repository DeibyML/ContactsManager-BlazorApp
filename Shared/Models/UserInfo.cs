using System.ComponentModel.DataAnnotations;

namespace BlazorCrud.Shared.Models;

public class UserInfo
{
    [MinLength(5)]
    [MaxLength(20)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}