using System.ComponentModel.DataAnnotations;


namespace API.DTOs
{
    public class RegisterDTo
    {
        [Required]
        [MinLength(5)]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; } 
    }
}