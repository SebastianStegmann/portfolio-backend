using System.ComponentModel.DataAnnotations;

public class LoginModel 
{ 
  [Required]
  [EmailAddress]
  public required string Email { get; set; } 

  [Required]
  public required string Password { get; set; }
}
