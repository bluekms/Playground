using System.ComponentModel.DataAnnotations;

namespace OperationServer.Pages.Accounts.ViewModels;

public class CredentialVM
{
    [Required]
    public string AccountId { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
