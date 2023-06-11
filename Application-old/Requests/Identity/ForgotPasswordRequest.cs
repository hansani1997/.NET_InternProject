using System.ComponentModel.DataAnnotations;

namespace BlueLotus360.Com.Application.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}