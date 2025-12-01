using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDevlights.Business.DTOs.Auth
{
    public class TokenResponseDto
    {
        public string? Token { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
