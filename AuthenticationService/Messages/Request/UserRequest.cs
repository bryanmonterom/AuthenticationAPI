using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Messages.Request;

public class UserRequest
{
    public UserRequest()
    {
    }
    [EmailAddress] [Required] public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,100}$",
        ErrorMessage =
            "Password must have at least one lowercase letter, one uppercase letter, one digit, and one special character")]
    public required string Password { get; set; }


}