namespace CommonLibrary.Models
{
    public enum UserRoles
    {
        Administrator,
        Developer,
        InternalService,
        WhitelistedUser,
        User,
    }

    public enum ServerRoles
    {
        Auth,
        Operation,
        World,
    }
}