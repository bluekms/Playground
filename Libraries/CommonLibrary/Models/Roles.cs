namespace CommonLibrary.Models
{
    public enum ClientRoles
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