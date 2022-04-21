using AccountOfTrafficViolationDB.Models;

namespace AccountOfTrafficViolationDB.ProxyModels;

public class UserInfo
{
    public int OfficerId { get; set; }
    
    public byte Role { get; set; }
    
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Phone { get; set; }

    public UserProfile? UserProfile { get; set; }
    
    public Officer ToOfficer()
    {
        return new Officer
        {
            Id = OfficerId,
            Name = Name,
            Surname = Surname,
            Phone = Phone
        };
    }
}