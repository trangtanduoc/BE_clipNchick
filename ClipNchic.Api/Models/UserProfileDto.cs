using ClipNchic.DataAccess.Models;

namespace ClipNchic.Api.Models;

public record UserProfileDto
{
    public int Id { get; init; }
    public string? Email { get; init; }
    public string? Name { get; init; }
    public string? Phone { get; init; }
    public DateTime? Birthday { get; init; }
    public string? Address { get; init; }
    public string? Image { get; init; }
    public DateTime? CreateDate { get; init; }
    public string? Status { get; init; }

    public static UserProfileDto FromEntity(User user) => new()
    {
        Id = user.id,
        Email = user.email,
        Name = user.name,
        Phone = user.phone,
        Birthday = user.birthday,
        Address = user.address,
        Image = user.image,
        CreateDate = user.createDate,
        Status = user.status
    };
}
