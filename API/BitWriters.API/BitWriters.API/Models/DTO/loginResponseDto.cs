namespace BitWriters.API.Models.DTO
{
    public class loginResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
