using System.ComponentModel.DataAnnotations;

namespace OperationServer.Pages.Accounts.ViewModels;

public class AccountCreateVM
{
    [Required(ErrorMessage = "Account Id is required.")]
    [MinLength(4, ErrorMessage = "Account Id must be at least 4 characters.")]
    [RegularExpression("^[a-zA-Z0-9-_]+$", ErrorMessage = "Account Id can only contain letters, numbers, hyphen (-), and underscore (_)")]
    public required string AccountId { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(4, ErrorMessage = "Password must be at least 4 characters.")]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    public ResSignUp.Types.AccountRoles Role { get; set; }
}
