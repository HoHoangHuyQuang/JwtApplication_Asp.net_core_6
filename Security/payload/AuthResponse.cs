﻿namespace JwtApplication.Security.payload
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
