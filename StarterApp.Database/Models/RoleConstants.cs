namespace StarterApp.Database.Models;

public static class RoleConstants
{
    public const string Admin = "admin";
    public const string OrdinaryUser = "ordinaryUser";
    public const string SpecialUser = "specialUser";
    
    public static readonly string[] AllRoles = { Admin, OrdinaryUser, SpecialUser };
}