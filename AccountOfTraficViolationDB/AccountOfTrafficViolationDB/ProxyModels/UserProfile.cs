namespace AccountOfTrafficViolationDB.ProxyModels;

public class UserProfile
{
    public string Login { get; set; }
    public string Password { get; set; }
    
    public UserInfo? UserInfo { get; set; }
}