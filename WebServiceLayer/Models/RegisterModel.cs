using System.ComponentModel.DataAnnotations;

public class RegisterModel 
{ 
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public required string Password { get; set; } 
}
