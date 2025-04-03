namespace Entity.DTOs
{
    public interface IRolUserDto
    {
        string Form { get; set; }
        int Id { get; set; }
        int Id { get; set; }
        bool IsDeleted { get; set; }
        string Name { get; set; }
        int RolId { get; set; }
        int RolUserId { get; set; }
        string Url { get; set; }
        int UserId { get; set; }
    }
}