namespace OMA_DuendeIdentityServer.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
