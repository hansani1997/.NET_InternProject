using System;

namespace BlueLotus360.Com.Application.Responses.Identity
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserImageURL { get; set; }
        public string Messages { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public bool IsSuccess { get; set; }
        public bool IsError {  get; set; }
    }
}